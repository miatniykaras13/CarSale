import {
	BadRequestException, Body,
	Controller,
	Get, Param, Put,
	Req,
	Res
} from '@nestjs/common';

import type { Request } from 'express';
import { ProfileService } from './profile.service';
import { AuthenticatedUser, Public } from 'nest-keycloak-connect'
import { CurrentUser } from '@/profile/decorators/current-user.decorator'
import { MeDto } from '@/profile/dto/me.dto'
import { User } from '@prisma/generated/client'
import { UpdateProfileDto } from '@/profile/dto/update-profile.dto'

@Controller('profiles')
export class ProfileController {
  	public constructor(private readonly profileService: ProfileService) {}

	@Get('me')
	public async getMe(@CurrentUser() tokenPayload: any): Promise<MeDto> {
        return await this.profileService.getMe(tokenPayload)
	}
	
	@Get('/:id')
	public async getProfileById(
		@Param('id') id: string,
		@Req() req: Request,
	) {
		  // console.log(req.headers.authorization)
		return await this.profileService.findById(id)
	}
	
	@Put('/me')
	public async updateProfile(
		@Body() dto: UpdateProfileDto,
		@Req() req: Request
	) {
		  if(!req.user)
			  throw new BadRequestException('Not logged in');
		  
		  const userId = req.user['sub']
		
		  return this.profileService.update(userId, dto)
	}
}