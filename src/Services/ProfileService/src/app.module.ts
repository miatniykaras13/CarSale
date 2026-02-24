import { Module } from '@nestjs/common';
import { AppController } from './app.controller';
import { AppService } from './app.service';
import { ProfileModule } from './profile/profile.module';
import { AdModule } from './ad/ad.module';
import { ConfigModule } from '@nestjs/config'
import { 
	KeycloakConnectModule,
	AuthGuard,
	ResourceGuard, RoleGuard
} from 'nest-keycloak-connect'
import { APP_GUARD } from '@nestjs/core'
import { ConfigModules } from './config/config.module';
import { KeycloakConfigService } from '@/config/keycloak-config.service'

@Module({
  imports: [ 
	  ProfileModule,
	  AdModule,
	  ConfigModule.forRoot({
		  isGlobal: true
	  }),
	  KeycloakConnectModule.registerAsync({
		  useExisting: KeycloakConfigService,
		  imports: [ConfigModules],
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
