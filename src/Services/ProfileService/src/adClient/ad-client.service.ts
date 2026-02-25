import { Injectable } from '@nestjs/common';
import { HttpService } from '@nestjs/axios';
import { firstValueFrom } from 'rxjs';

@Injectable()
export class AdClientService {
	constructor(private readonly httpService: HttpService) {}

	async getUserAds(userId: string, token: string) {
		const response = await firstValueFrom(
			this.httpService.get(
				`http://localhost:3000/ads/user/${userId}`,
				{
					headers: {
						Authorization: `Bearer ${token}`,
					},
				},
			),
		);

		return response.data;
	}
}