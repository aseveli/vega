import { IMake } from '../vehicle-form/make';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { IFeature } from '../vehicle-form/feature';

@Injectable({
	providedIn: 'root'
})
export class VehicleService {
	constructor(private http: HttpClient) {}

	getMakes(): Observable<IMake[]> {
		return this.http.get<IMake[]>('/api/makes');
	}

	getFeatures(): Observable<IFeature[]> {
		console.log('FEATURES');
		return this.http.get<IFeature[]>('/api/features');
	}
}
