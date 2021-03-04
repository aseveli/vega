import { IContact } from "./icontact";

export interface ISaveVehicle {
  id: number;
  makeId: number;
  modelId: number;
  isRegistered: boolean;
  features: number[];
  contact: IContact;
}
