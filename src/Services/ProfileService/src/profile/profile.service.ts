 import { Injectable, NotFoundException } from '@nestjs/common'

import { PrismaService } from '@/prisma/prisma.service'

import { UpdateUserDto } from './dto/update-user.dto'

@Injectable()
export class ProfileService {

	public constructor(private readonly prismaService: PrismaService) {}

	public async findById(id: string) {
		const user = await this.prismaService.user.findUnique({
			where: {
				id
			},
			select: {
				email: true,
				name: true,
				id: true
			}
		})

		if (!user) {
			throw new NotFoundException(
				'Пользователь не найден. Пожалуйста, проверьте введенные данные.'
			)
		}

		return user
	}

	public async findByEmail(email: string) {
		const user = await this.prismaService.user.findUnique({
			where: {
				email
			},
			select: {
				email: true,
				name: true,
				id: true
			}
		})

		return user
	}

	public async create(
		keycloakId: string,
		email: string,
		name: string,
		picture: string
	) {
		// const user = 
		await this.prismaService.user.create({
			data: {
				keycloakId,
				email,
				name,
				picture
			}
		})

		return true
	}

	public async update(userId: string, dto: UpdateUserDto) {
		const user = await this.findById(userId)

		// const updatedUser = 
		await this.prismaService.user.update({
			where: {
				id: user.id
			},
			data: {
				email: dto.email,
				name: dto.name
			}
		})

		return true
	}
}