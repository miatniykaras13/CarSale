import { Module } from '@nestjs/common';
import { AppController } from './app.controller';
import { AppService } from './app.service';
import { ProfileModule } from './profile/profile.module';
import { ConfigModule } from '@nestjs/config';
import { APP_GUARD } from '@nestjs/core';
import { HttpModule } from '@nestjs/axios';
import { join } from 'path';
import { AuthModule } from '@/auth/auth.module';
import { JwtAuthGuard } from '@/auth/jwt-auth.guard';

@Module({
	imports: [
		ProfileModule,
		AuthModule,
		ConfigModule.forRoot({
			isGlobal: true,
			envFilePath: [
				join(__dirname, '..', '..', '..', '.env'),
				join(process.cwd(), '..', '..', '..', '.env'),
			],
		}),
		HttpModule.register({
			timeout: 5000,
			maxRedirects: 5,
		}),
	],
	controllers: [AppController],
	providers: [
		AppService,
		{
			provide: APP_GUARD,
			useClass: JwtAuthGuard,
		},
	],
})
export class AppModule {}
