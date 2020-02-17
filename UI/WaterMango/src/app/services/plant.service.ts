import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PlantService {

  api: string = environment.API_BASE + "Plant/";

  constructor( 
    private http: HttpClient
  ) { }

  getPlants() {
    return this.http.get(this.api);
  }

  startWateringPlant(id:any) {
    const endpoint = this.api + id + "/start";

    return this.http.post(endpoint, null).subscribe();
  }

  stopWateringPlant(id:any) {
    const endpoint = this.api + id + "/stop";

    return this.http.post(endpoint, null).subscribe();
  }
}
