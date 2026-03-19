import { Module } from '@nestjs/common';
import { ProfileService } from './profile.service';
import { ProfileController } from './profile.controller';
import { PrismaModule } from '@/prisma/prisma.module'
import { KeycloakAdminService } from '@/keycloak/keycloak-admin.service'

@Module({
  controllers: [ProfileController],
  providers: [ProfileService, KeycloakAdminService],
  imports: [PrismaModule],
})
export class ProfileModule {}