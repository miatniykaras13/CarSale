import { Injectable } from '@nestjs/common';
import { ConfigService } from '@nestjs/config';
import { PassportStrategy } from '@nestjs/passport';
import { passportJwtSecret } from 'jwks-rsa';
import { ExtractJwt, Strategy } from 'passport-jwt';

@Injectable()
export class JwtStrategy extends PassportStrategy(Strategy) {
	constructor(configService: ConfigService) {
		const realm = configService.getOrThrow<string>('KEYCLOAK_REALM');
		const publicUrl = trimTrailingSlash(
			configService.getOrThrow<string>('KEYCLOAK_URL'),
		);
		const internalUrl = trimTrailingSlash(
			configService.get<string>('KEYCLOAK_INTERNAL_URL') ?? publicUrl,
		);

		super({
			jwtFromRequest: ExtractJwt.fromAuthHeaderAsBearerToken(),
			ignoreExpiration: false,
			issuer: `${publicUrl}/realms/${realm}`,
			algorithms: ['RS256'],
			secretOrKeyProvider: passportJwtSecret({
				cache: true,
				rateLimit: true,
				jwksRequestsPerMinute: 5,
				jwksUri: `${internalUrl}/realms/${realm}/protocol/openid-connect/certs`,
			}),
		});
	}

	validate(payload: Record<string, unknown>) {
		return payload;
	}
}

function trimTrailingSlash(url: string): string {
	return url.replace(/\/+$/, '');
}
