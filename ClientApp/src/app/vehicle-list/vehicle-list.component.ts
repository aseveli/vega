import { VehicleService } from "./../services/vehicle.service";
import { IVehicle } from "./../vehicle-form/ivehicle";
import { Component, OnInit } from "@angular/core";
import { IMake } from "../vehicle-form/imake";
import { IModel } from "../vehicle-form/imodel";

@Component({
  selector: "app-vehicle-list",
  templateUrl: "./vehicle-list.component.html",
  styleUrls: ["./vehicle-list.component.css"],
})
export class VehicleListComponent implements OnInit {
  vehicles: IVehicle[];
  makes: IMake[];
  models: IModel[];
  filteredModels: IModel[];
  query: any = {};
  columns = [
    { title: "Id" },
    { title: "Contact Name", key: "contactName", isSortable: true },
    { title: "Make", key: "make", isSortable: true },
    { title: "Model", key: "model", isSortable: true },
    {},
  ];

  constructor(private vehicleService: VehicleService) {}

  ngOnInit(): void {
    this.vehicleService.getMakes().subscribe((makes) => (this.makes = makes));

    this.vehicleService
      .getModels()
      .subscribe((models) => (this.models = models));

    this.populateVehicles();
  }
  private populateVehicles() {
    console.log("filtering: " + JSON.stringify(this.query));

    this.vehicleService
      .getVehicles(this.query)
      .subscribe((vehicles) => (this.vehicles = vehicles));
  }
  onModelFilterChange() {
    delete this.query.modelId;

    this.filteredModels = this.models.filter(
      (m) => m.makeId == this.query.makeId
    );

    this.onFilterChange();
  }
  onFilterChange() {
    this.populateVehicles();
  }
  resetFilter() {
    this.query = {};
    this.onFilterChange();
  }
  sortBy(columnName) {
    if (this.query.sortBy === columnName) {
      this.query.isSortAscending = !this.query.isSortAscending;
    } else {
      this.query.sortBy = columnName;
      this.query.isSortAscending = true;
    }

    this.populateVehicles();
  }
}
