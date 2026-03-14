import { Module } from '@nestjs/common';
import { AppController } from './app.controller';
import { AppService } from './app.service';
import { ProfileModule } from './profile/profile.module';
import { ConfigModule } from '@nestjs/config'
import { 
	KeycloakConnectModule,
	AuthGuard,
	ResourceGuard, RoleGuard
} from 'nest-keycloak-connect'
import { APP_GUARD } from '@nestjs/core'
import { ConfigModules } from './config/config.module';
import { KeycloakConfigService } from '@/config/keycloak-config.service'
import { HttpModule } from '@nestjs/axios'
import { join } from 'path'

@Module({
  imports: [ 
	  ProfileModule,
	  ConfigModule.forRoot({
		  isGlobal: true,
		  envFilePath: [
			  join(__dirname, '..', '..', '..', '.env'),
			  join(process.cwd(), '..', '..', '..', '.env')
		  ]
	  }),
	  KeycloakConnectModule.registerAsync({
		  useClass: KeycloakConfigService,
		  imports: [ConfigModules, ConfigModule],
	  }),
	  HttpModule.register({
		  timeout: 5000,
		  maxRedirects: 5
	  }),
	  ConfigModule
  ],
  controllers: [AppController],
  providers: [
	  AppService,

	  {
		  provide: APP_GUARD,
		  useClass: AuthGuard
	  },
	  {
		  provide: APP_GUARD,
		  useClass: ResourceGuard
	  },
	  {
		  provide: APP_GUARD,
		  useClass: RoleGuard
	  }
  ],
})
export class AppModule {}
