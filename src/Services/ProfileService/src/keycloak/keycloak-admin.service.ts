import { Injectable, OnModuleInit } from '@nestjs/common';
import type KcAdminClient from '@keycloak/keycloak-admin-client';

@Injectable()
export class KeycloakAdminService implements OnModuleInit {
	private kcAdminClient: KcAdminClient;

	async onModuleInit() {
		const { default: KcAdminClientClass } = await (eval(`import('@keycloak/keycloak-admin-client')`) as Promise<any>);

		this.kcAdminClient = new KcAdminClientClass({
			baseUrl: 'http://keycloak:8080',
			realmName: 'carsale',
		});

		await this.kcAdminClient.auth({
			grantType: 'client_credentials',
			clientId: 'profile-service',
			clientSecret: 'cOtGUlHQS2KjbhMakmTOpAZNxBIczfSP',
		});
	}

	async updateUser(id: string, data: any) {
		return await this.kcAdminClient.users.update({ id }, data);
	}

	async deleteUser(id: string) {
		try {
			await this.kcAdminClient.users.del({ id })

			return {
				success: true
			}
		} catch (error) {
			throw new Error(`Failed to delete user ${id}`)
		}
	}
}