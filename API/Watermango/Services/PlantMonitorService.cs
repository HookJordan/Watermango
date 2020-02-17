
using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Watermango.Repository;
using Watermango.Models;

namespace Watermango.Services {
    public class PlantMonitorService : IHostedService
     {
        private Timer timer; 
        private IServiceScopeFactory scopeFactory;

        public PlantMonitorService(IServiceScopeFactory scopeFactory) 
        {
            this.scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken stoppingToken) 
        {
            this.timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
            
            return Task.CompletedTask;
        }

        private void DoWork(object state) 
        {
            // Find the plant repository
            using (var scope = this.scopeFactory.CreateScope())
            {
                var plantRepository = scope.ServiceProvider.GetRequiredService<PlantRepository>();

                // Check on plants
                var plantList = plantRepository.GetPlantsNotIdle();
                foreach(Plant plant in plantList)
                {
                    var difference = (DateTime.Now - plant.LastUpdated).TotalSeconds;

                    // Prevent over watering
                    if (plant.State == PlantStatus.Watering) 
                    {
                        if (difference >= 10) 
                        {
                            plantRepository.UpdateStatus(plant.Id, PlantStatus.Resting);
                        }
                    }
                    // Release plants when resting is complete
                    else if (plant.State == PlantStatus.Resting)
                    {
                        if (difference >= 30) 
                        {
                            plantRepository.UpdateStatus(plant.Id, PlantStatus.Idle);
                        }
                    }
                }
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            this.timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
    }
}