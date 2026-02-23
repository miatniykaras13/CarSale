import { 
	Controller,
	Get, Param,
	Req,
	Res
} from '@nestjs/common';
import { ProfileService } from './profile.service';

@Controller('profiles')
export class ProfileController {
  	public constructor(private readonly profileService: ProfileService) {}
	
	@Get('/:id')
	public async getProfileById(
		@Param('id') id: string
	) {
		return this.profileService.findById(id);
	}
}
