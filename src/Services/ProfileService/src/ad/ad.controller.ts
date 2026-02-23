import { Controller } from '@nestjs/common';
import { AdService } from './ad.service';

@Controller('ad')
export class AdController {
  constructor(private readonly adService: AdService) {}
}
