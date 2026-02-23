import { Module } from '@nestjs/common';
import { AppController } from './app.controller';
import { AppService } from './app.service';
import { AuthModule } from './src/auth/auth.module';
import { AuthModule } from './auth/auth.module';
import { ProfileModule } from './user/profile.module';
import { AdModule } from './ad/ad.module';

@Module({
  imports: [AuthModule, ProfileModule, AdModule],
  controllers: [AppController],
  providers: [AppService],
})
export class AppModule {}
