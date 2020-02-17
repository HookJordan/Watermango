using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Watermango.Models;
using System;

namespace Watermango.Repository 
{
    public class PlantRepository 
    {
        private readonly PlantContext context;

        public PlantRepository(PlantContext context) 
        {
            this.context = context;

            if (context.Plants.Count() == 0)
            {
                context.Plants.AddRange(
                    new Plant
                    {
                        Id = 1,
                        Name = "Cactus",
                        Location = "Roof",
                        PreviewUrl = "http://icons.iconarchive.com/icons/google/noto-emoji-animals-nature/256/22332-cactus-icon.png"
                    },
                    new Plant
                    {
                        Id = 2,
                        Name = "Roses",
                        Location = "2nd Floor",
                        PreviewUrl = "http://icons.iconarchive.com/icons/aha-soft/free-large-love/256/Rose-icon.png"
                    },
                    new Plant
                    {
                        Id = 3,
                        Name = "Tulip",
                        Location = "3nd Floor",
                        PreviewUrl = "http://icons.iconarchive.com/icons/google/noto-emoji-animals-nature/256/22327-tulip-icon.png"

                    },
                    new Plant
                    {
                        Id = 4,
                        Name = "Wolfsbane",
                        Location = "Basement",
                        LastUpdated = DateTime.Now.AddHours(-6),
                        PreviewUrl = "https://wiki.gamedetectives.net/images/c/c6/Wolfsbane.png"
                    },
                    new Plant
                    {
                        Id = 5,
                        Name = "Venus Fly Trap",
                        Location = "Main Entrance",
                        LastUpdated = DateTime.Now.AddHours(-6),
                        PreviewUrl = "https://p1.hiclipart.com/preview/605/791/711/super-mario-icons-carnivore-plant-thumbnail.jpg"
                    }
                );

                if (context.Plants.Count() == 0)
                {
                    // Persist DB
                    context.SaveChanges();
                }
            }
        }

        public List<Plant> GetPlants() 
        {
            return context.Plants.OrderBy(p => p.Id).ToList();
        }

        public List<Plant> GetPlantsNotIdle() 
        {
            return context.Plants.Where(p => p.State != PlantStatus.Idle).ToList();
        }

        public Plant GetPlantById(long id) 
        {
            Plant p = context.Plants.Find(id);

            return p;
        }

        public Plant UpdateStatus(long id, PlantStatus status) 
        {
            Plant p = this.GetPlantById(id);

            if (p != null) {
                p.State = status;
                p.LastUpdated = DateTime.Now;
                context.Plants.Update(p);
                context.SaveChanges();
            }

            return this.GetPlantById(id);
        }
    }
}