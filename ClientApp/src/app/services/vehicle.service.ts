import { IVehicle } from "../vehicle-form/ivehicle";
import { IMake } from "../vehicle-form/imake";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { IFeature } from "../vehicle-form/ifeature";
import { ISaveVehicle } from "../vehicle-form/isavevehicle";
import { IModel } from "../vehicle-form/imodel";

@Injectable({
  providedIn: "root",
})
export class VehicleService {
  private readonly vehilcesEndpoint = "/api/vehicles";
  constructor(private http: HttpClient) {}

  toQueryString(obj) {
    var parts = [];

    for (var property in obj) {
      var value = obj[property];

      if (value != null && value != undefined) {
        parts.push(
          encodeURIComponent(property) + "=" + encodeURIComponent(value)
        );
      }
    }

    let qryString = parts.join("&");

    console.log("toQueryString: " + qryString);
    return qryString;
  }
  getVehicles(filter) {
    return this.http.get<IVehicle[]>(
      this.vehilcesEndpoint + "?" + this.toQueryString(filter)
    );
  }
  getVehicle(id) {
    return this.http.get<IVehicle>(this.vehilcesEndpoint + "/" + id);
  }
  createVehicle(vehicle: ISaveVehicle) {
    var newVehicle = this.http.post<ISaveVehicle>(
      this.vehilcesEndpoint,
      vehicle
    );

    return newVehicle;
  }
  deleteVehicle(id) {
    return this.http.delete(this.vehilcesEndpoint + "/" + id);
  }
  getMakes(): Observable<IMake[]> {
    return this.http.get<IMake[]>("/api/makes");
  }
  getModels(): Observable<IModel[]> {
    return this.http.get<IModel[]>("/api/models");
  }
  getFeatures(): Observable<IFeature[]> {
    return this.http.get<IFeature[]>("/api/features");
  }
  updateVehicle(vehicle: ISaveVehicle) {
    return this.http.put<ISaveVehicle>(
      this.vehilcesEndpoint + "/" + vehicle.id,
      vehicle
    );
  }
}
