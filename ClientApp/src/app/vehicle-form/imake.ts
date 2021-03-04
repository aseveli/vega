import { IModel } from "./imodel";

export interface IMake {
  id: number;
  name: string;
  models: IModel[];
}
