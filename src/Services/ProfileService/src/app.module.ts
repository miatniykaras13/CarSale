import { Module } from '@nestjs/common';
import { AppController } from './app.controller';
import { AppService } from './app.service';
import { ProfileModule } from './profile/profile.module';
import { AdModule } from './ad/ad.module';
import { ConfigModule } from '@nestjs/config'

@Module({
  imports: [ 
	  ProfileModule,
	  AdModule,
	  ConfigModule.forRoot({
		  isGlobal: true
	  })
  ],
  controllers: [AppController],
  providers: [AppService],
})
export class AppModule {}
