import { NestFactory } from '@nestjs/core';
import { AppModule } from './app.module';
import { ValidationPipe } from '@nestjs/common'
import { fillDb } from '@/fill-db'
import { PrismaService } from '@/prisma/prisma.service'

async function bootstrap() {
	const app = await NestFactory.create(AppModule)
	
	app.useGlobalPipes(new ValidationPipe({
	  whitelist: true,
	  forbidNonWhitelisted: true,
	  transform: true
	}))
	
	// await fillDb(new PrismaService())
	
	await app.listen(process.env.PORT ?? 4000)
}
bootstrap()
