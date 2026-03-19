import { Injectable } from '@nestjs/common';
import {
	KeycloakConnectOptions,
	KeycloakConnectOptionsFactory,
	PolicyEnforcementMode,
	TokenValidation
} from 'nest-keycloak-connect';
import { ConfigService } from '@nestjs/config';

@Injectable()
export class KeycloakConfigService implements KeycloakConnectOptionsFactory {
	constructor(private readonly configService: ConfigService) {}

	createKeycloakConnectOptions(): KeycloakConnectOptions {
		return {
			authServerUrl: this.configService.getOrThrow<string>('KEYCLOAK_URL'),
			realm: this.configService.getOrThrow<string>('KEYCLOAK_REALM'),
			clientId: this.configService.getOrThrow<string>('KEYCLOAK_CLIENT_ID'),
			secret: '',
			tokenValidation: TokenValidation.OFFLINE,
			policyEnforcement: PolicyEnforcementMode.PERMISSIVE,
			logLevels: ['debug'],
			useNestLogger: true,
		};
	}
}