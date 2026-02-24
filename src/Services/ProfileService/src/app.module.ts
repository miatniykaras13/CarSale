import { Module } from '@nestjs/common';
import { AppController } from './app.controller';
import { AppService } from './app.service';
import { ProfileModule } from './profile/profile.module';
import { AdModule } from './ad/ad.module';
import { ConfigModule } from '@nestjs/config'
import { KeycloakConnectModule } from 'nest-keycloak-connect'

@Module({
  imports: [ 
	  ProfileModule,
	  AdModule,
	  ConfigModule.forRoot({
		  isGlobal: true
	  }),
	  // KeycloakConnectModule.register({
		//  
	  // })
  ],
  controllers: [AppController],
  providers: [AppService],
})
export class AppModule {}
