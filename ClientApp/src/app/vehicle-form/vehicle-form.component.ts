import { ToastrService } from "ngx-toastr";
import * as _ from "underscore";
import { IVehicle } from "./ivehicle";
import { forkJoin } from "rxjs";
import { IFeature } from "./ifeature";
import { IModel } from "./imodel";
import { IMake } from "./imake";
import { VehicleService } from "../services/vehicle.service";
import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { ISaveVehicle } from "./isavevehicle";

@Component({
  selector: "app-vehicle-form",
  templateUrl: "./vehicle-form.component.html",
  styleUrls: ["./vehicle-form.component.css"],
})
export class VehicleFormComponent implements OnInit {
  makes: IMake[];
  models: IModel[];
  features: IFeature[];
  vehicle: ISaveVehicle = {
    id: 0,
    makeId: 0,
    modelId: 0,
    isRegistered: false,
    features: [],
    contact: {
      name: "",
      phone: "",
      email: "",
    },
  };

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private toastrService: ToastrService,
    private vehicleService: VehicleService
  ) {
    this.route.params.subscribe((p) => {
      this.vehicle.id = +p["id"] || 0;
    });
  }

  ngOnInit(): void {
    if (!this.vehicle.id) {
      forkJoin({
        requestMakes: this.vehicleService.getMakes(),
        requestFeatures: this.vehicleService.getFeatures(),
      }).subscribe(
        ({ requestMakes, requestFeatures }) => {
          this.makes = requestMakes;
          this.features = requestFeatures;
        },
        (err) => {
          if (err.status == 404) {
            this.router.navigate(["home"]);
          }
        }
      );
    } else {
      forkJoin({
        requestMakes: this.vehicleService.getMakes(),
        requestFeatures: this.vehicleService.getFeatures(),
        requestVehicle: this.vehicleService.getVehicle(this.vehicle.id),
      }).subscribe(
        ({ requestMakes, requestFeatures, requestVehicle }) => {
          this.makes = requestMakes;
          this.features = requestFeatures;
          this.setVehicle(requestVehicle);
          this.populateModels();
        },
        (err) => {
          if (err.status == 404) {
            this.router.navigate(["home"]);
          }
        }
      );
    }
  }
  private setVehicle(v: IVehicle) {
    this.vehicle.id = v.id;
    this.vehicle.makeId = v.make.id;
    this.vehicle.modelId = v.model.id;
    this.vehicle.isRegistered = v.isRegistered;
    this.vehicle.contact = v.contact;
    this.vehicle.features = _.pluck(v.features, "id");
  }
  onMakeChange() {
    this.populateModels();

    //need to clear out the modelId when the Make changes
    delete this.vehicle.modelId;
  }
  private populateModels() {
    //find a make that matches our makeid
    var selectedMake = this.makes.find((m) => m.id == this.vehicle.makeId);

    //then set our models property to the model of the make we found, and if
    //the make is null or undefined than we use an empty array
    this.models = selectedMake ? selectedMake.models : [];
  }
  onFeatureToggle(featureId, $event) {
    if ($event.target.checked) {
      this.vehicle.features.push(featureId);
    } else {
      var index = this.vehicle.features.indexOf(featureId);

      this.vehicle.features.splice(index, 1);
    }
  }
  submit() {
    var result$ = this.vehicle.id
      ? this.vehicleService.updateVehicle(this.vehicle)
      : this.vehicleService.createVehicle(this.vehicle);

    result$.subscribe(
      null,
      (error) => {
        console.log("Error");
      },
      () => {
        this.toastrService.success(
          "Vehicle Data was saved successfully.",
          "Success",
          {
            onActivateTick: true,
            closeButton: true,
            timeOut: 5000,
          }
        );
      }
    );

    this.router.navigate(["home"]);

    // if (this.vehicle.id) {
    //   this.vehicleService.updateVehicle(this.vehicle).subscribe((vehicle) => {
    //     this.toastrService.success();
    //     //this.router.navigate["/vehicles" + vehicle.id];
    //     this.router.navigate["/home"];
    //   });
    // } else {
    //   this.vehicleService.createVehicle(this.vehicle).subscribe((vehicle) => {
    //     this.toastrService.success();
    //     //this.router.navigate["/vehicles" + vehicle.id];
    //     this.router.navigate["/home"];
    //   });
    // }
  }
  delete() {
    if (confirm("Are you sure?")) {
      this.vehicleService
        .deleteVehicle(this.vehicle.id)
        .subscribe((x) => this.router.navigate["home"]);
    }
  }
}
