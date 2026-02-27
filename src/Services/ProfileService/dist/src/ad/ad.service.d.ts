import { PrismaService } from '@/prisma/prisma.service';
export declare class AdService {
    private readonly prismaService;
    constructor(prismaService: PrismaService);
    findAllByUserId(userId: string): Promise<{
        id: string;
        userId: string;
        title: string;
        description: string | null;
        carId: string;
        city: string;
        region: string;
        costAmount: number;
        currencyCode: string;
    }[]>;
}
