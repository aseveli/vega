import { IModel } from "./imodel";
import { IContact } from "./icontact";
import { IFeature } from "./ifeature";
import { IMake } from "./imake";

export interface IVehicle {
  id: number;
  model: IModel;
  make: IMake;
  isRegistered: boolean;
  features: IFeature[];
  contact: IContact;
  lastUpdate: string;
}
