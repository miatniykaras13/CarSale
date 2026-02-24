import { 
	Controller,
	Get, Param,
	Req,
	Res
} from '@nestjs/common';

import type { Request } from 'express';
import { ProfileService } from './profile.service';

@Controller('profiles')
export class ProfileController {
  	public constructor(private readonly profileService: ProfileService) {}
	
	@Get('/:id')
	public async getProfileById(
		@Param('id') id: string,
		@Req() req: Request
	) {
		  console.log(req.headers.authorization)
		return this.profileService.findById(id)
	}
}