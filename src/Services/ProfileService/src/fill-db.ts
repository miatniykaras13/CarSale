import { PrismaService } from '@/prisma/prisma.service'
import { BadRequestException } from '@nestjs/common'

export async function fillDb(prismaService: PrismaService) {
	const user = await prismaService.user.findFirst()

	if (!user)
		throw new BadRequestException("User not created to fill database")

	const ad = await prismaService.adSnapshot.findFirst({})

	if(ad)
		return null

	const car = await prismaService.carSnapshot.create({
		data: {
			brand: "BMW",
			model: "M5 F90",
			generation: "2",
			year: 2019,
			driveType: "rear",
			transmissionType: "MT",
			engineVolume: 4.4,
			fuelType: "petrol",
			bodyType: "sedan"
		}
	})

	await prismaService.adSnapshot.create({
		data: {
			userId: user.id,
			title: "BMW sale",
			carId: car.id,
			city: "Moscow",
			region: "Russia",
			costAmount: 70000,
			currencyCode: "USD"
		}
	})
}