import {
	BadRequestException, Body,
	Controller, Delete,
	Get, HttpCode, Param, Patch, Post, Put, Query,
	Req,
	Res, UploadedFile, UseInterceptors
} from '@nestjs/common'

import type { Request } from 'express';
import { ProfileService } from './profile.service';
import { AuthenticatedUser, Public } from 'nest-keycloak-connect'
import { CurrentUser } from '@/profile/decorators/current-user.decorator'
import { MeDto } from '@/profile/dto/me.dto'
import { User } from '@prisma/generated/client'
import { UpdateProfileDto } from '@/profile/dto/update-profile.dto'
import { KeycloakAdminService } from '@/keycloak/keycloak-admin.service'
import { FileInterceptor } from '@nestjs/platform-express'

@Controller('profiles')
export class ProfileController {
  	public constructor(
		  private readonly profileService: ProfileService,
		  private readonly keycloakAdminService: KeycloakAdminService
		  ) {}

	@Get('/me')
	public async getMe(@CurrentUser() tokenPayload: any): Promise<MeDto> {
        return await this.profileService.getMe(tokenPayload)
	}
	
	@Put('/me')
	public async updateProfile(
		@Body() dto: UpdateProfileDto,
		@CurrentUser() tokenPayload: any
	) {
		return this.profileService.update(tokenPayload.sub, dto)
	}
    
    @Post('/batch')
    public async batchProfiles(
        @Body() ids: string[]
    ) {
          return await this.profileService.batchProfiles(ids)
    }
    
    @Get()
    public async findByUsername(
        @Query('username') username: string
    ) {
          return await this.profileService.findByUsername(username)
    }
    
    @Delete('/me')
	@HttpCode(204)
    public async deleteMe(@CurrentUser() tokenPayload: any) {
          await this.profileService.deleteUser(tokenPayload.sub)
    }
    
    @Get('exists/:id')
    public async exists(
        @Param('id') id: string
    ) {
          return await this.profileService.exists(id)
    }
	
	@Get('/search')
	public async search(
		@CurrentUser() tokenPayload: any,
		@Query('username') username: string
	) {
		  return this.profileService.searchByUsername(username, tokenPayload.sub)
	}
	
	@Public()
	@Get('/:id/short')
	public async short(
		@Param('id') id: string
	) {
		  return this.profileService.getShortById(id)
	}

	@Get('/:id')
	public async getProfileById(
		@Param('id') id: string,
		@Req() req: Request,
	) {
		// console.log(req.headers.authorization)
		return await this.profileService.findById(id)
	}
}