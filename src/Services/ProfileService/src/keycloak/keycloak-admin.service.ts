import { Injectable, OnModuleInit } from '@nestjs/common';
import type KcAdminClient from '@keycloak/keycloak-admin-client';
import { ConfigService } from '@nestjs/config'

@Injectable()
export class KeycloakAdminService implements OnModuleInit {
	private kcAdminClient: KcAdminClient;

	constructor(private readonly configService: ConfigService) {}

	async onModuleInit() {
		const { default: KcAdminClientClass } = await (eval(`import('@keycloak/keycloak-admin-client')`) as Promise<any>);

		const baseUrl =
			this.configService.get<string>('KEYCLOAK_INTERNAL_URL') ??
			this.configService.getOrThrow<string>('KEYCLOAK_URL');

		this.kcAdminClient = new KcAdminClientClass({
			baseUrl,
			realmName: this.configService.getOrThrow<string>('KEYCLOAK_REALM'),
		});

		await this.kcAdminClient.auth({
			grantType: 'client_credentials',
			clientId: this.configService.getOrThrow<string>('KEYCLOAK_ADMIN_CLIENT'),
			clientSecret: this.configService.getOrThrow<string>('KEYCLOAK_ADMIN_SECRET'),
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