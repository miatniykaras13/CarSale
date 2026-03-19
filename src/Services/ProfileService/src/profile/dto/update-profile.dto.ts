import { IsBoolean, IsEmail, IsNotEmpty, IsString } from 'class-validator'


export class UpdateProfileDto {

	@IsString({ message: 'Email должен быть строкой.' })
	@IsEmail({}, { message: 'Некорректный формат email.' })
	@IsNotEmpty({ message: 'Email обязателен для заполнения.' })
	email: string

	@IsString({ message: 'Имя должно быть строкой.' })
	@IsNotEmpty({ message: 'Имя пользователя обязательно для заполнения.' })
	username: string

	@IsString({ message: 'Имя должно быть строкой.' })
	@IsNotEmpty({ message: 'Имя обязательно для заполнения.' })
	name: string

	@IsString({ message: 'Имя должно быть строкой.' })
	@IsNotEmpty({ message: 'Фамилия обязательна для заполнения.' })
	surname: string
}