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
  plantsPoll = interval(500);
  plants: any[]; // Could define actual model object for plants...

  constructor(
    private plantService: PlantService
  ) { }

  ngOnInit() {
    // Initial poll
    this.plantService.getPlants()
    .subscribe(data => this.plants = data as any[])
    .add(() => { 
      this.refreshing = false;
      this.checkForExpiredPlants(); 
      this.refreshPlants(); 
    });

    // This could be done differently with web sockets or server sent events...
    this.plantsPoll.subscribe(n => {
      this.refreshPlants();
    });
  }

  refreshing: boolean = true;
  refreshPlants() {
    if (this.refreshing) { return; }
    this.refreshing = true; 
    this.plantService.getPlants().subscribe(data => this.plants = data as any[]).add(() => { 
      this.refreshing = this.checkForExpiredPlants(); 
    });

  }

  checkForExpiredPlants() {
    const list: any[] = [ ];
    this.plants.forEach((plant) => {
      const date = new Date(plant.lastUpdated).getTime();
      const now = new Date().getTime();

      // Get 6 hour diff 
      let diff = ((Math.abs(now - date) / 1000) / 60) / 60;

      if (diff >= 6) {
        list.push(plant.name);
      }
    });


    Swal.fire('Warning', list.join(', ') + " need to be watered ASAP!", 'warning')
    .then((result) => {
      this.refreshing = false;
    });

    return list.length > 0;
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

    this.clearSelected();
  }

  stopSelected() {
    this.selectedPlants.forEach((id)=> {
      this.plantService.stopWateringPlant(id);
    });

    this.clearSelected();
  }
}
