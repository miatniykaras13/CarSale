import { Injectable, NotFoundException } from '@nestjs/common';
import { PrismaService } from '@/prisma/prisma.service'

@Injectable()
export class AdService {
    
    public constructor(private readonly prismaService: PrismaService) {}
    
    public async findAllByUserId(userId: string) {
        return await this.prismaService.adSnapshot.findMany({
            where: {
                userId: userId
            }
        })
    }
}
