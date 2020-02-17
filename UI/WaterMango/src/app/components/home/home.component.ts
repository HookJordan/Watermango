import { Component, OnInit } from '@angular/core';
import { forkJoin, timer, interval } from 'rxjs';
import { PlantService } from 'src/app/services/plant.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  selectedPlants: any[] = [ ];
  plantsPoll = interval(1000);
  plants: any[] = [ ]; // Could define actual model object for plants...
  dyingPlants: any[] = [ ];

  constructor(
    private plantService: PlantService
  ) { }

  ngOnInit() {
    // This could be done differently with web sockets or server sent events...
    this.plantsPoll.subscribe(n => {
      this.refreshPlants();
    });
  }

  refreshPlants() {
    this.plantService.getPlants().subscribe(data => this.plants = data as any[]).add(() => { 
      this.checkForExpiredPlants(); 
    });

  }

  checkForExpiredPlants() {
    // const list: any[] = [ ];
    this.dyingPlants = [ ];
    this.plants.forEach((plant) => {
      const date = new Date(plant.lastUpdated).getTime();
      const now = new Date().getTime();

      // Get 6 hour diff 
      let diff = ((Math.abs(now - date) / 1000) / 60) / 60;

      if (diff >= 6) {
        this.dyingPlants.push(plant.name);
      }
    });
  }

  typeState(state) {
    if (state == 1) {
      return 'Idle';
    } else if (state == 2) {
      return 'Watering';
    } else if (state == 3) {
      return 'Resting';
    } else {
      return 'Error';
    }
  }

  isSelected(id) {
    return this.selectedPlants.indexOf(id) !== -1;
  }

  toggleSelect(plant) {
    // 3 = Resting -- don't touch a resting plant!
    if (plant.state == 3) { return; }

    const idx = this.selectedPlants.indexOf(plant.id);
    if(idx === -1) {
      this.selectedPlants.push(plant.id);
    } else {
      this.selectedPlants.splice(idx, 1);
    }
  }

  clearSelected() {
    this.selectedPlants = [ ];
  }


  // The following functions could be converted to a bulk call to the endpoint 
  // Where a the array of id's is passed to the endpoint instead of doing 1 at a time
  startSelected() {
    this.selectedPlants.forEach((id)=> {
      this.plantService.startWateringPlant(id);
    });

    Swal.fire('Success', 'Request to start watering sent!', 'success')

    this.clearSelected();
  }

  stopSelected() {
    this.selectedPlants.forEach((id)=> {
      this.plantService.stopWateringPlant(id);
    });

    Swal.fire('Information', 'Request to stop watering sent!', 'info')

    this.clearSelected();
  }
}
