-- CreateTable
CREATE TABLE "User" (
    "id" TEXT NOT NULL,
    "keycloakId" TEXT NOT NULL,
    "email" TEXT NOT NULL,
    "username" TEXT NOT NULL,
    "name" TEXT NOT NULL,
    "surname" TEXT NOT NULL,
    "picture" TEXT,
    "createdAt" TIMESTAMP(3) NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "updatedAt" TIMESTAMP(3) NOT NULL,

    CONSTRAINT "User_pkey" PRIMARY KEY ("id")
);

-- CreateTable
CREATE TABLE "AdSnapshot" (
    "id" TEXT NOT NULL,
    "userId" TEXT NOT NULL,
    "title" TEXT,
    "description" TEXT,
    "carId" TEXT,
    "city" TEXT,
    "region" TEXT,
    "costAmount" INTEGER,
    "currencyCode" TEXT,

    CONSTRAINT "AdSnapshot_pkey" PRIMARY KEY ("id")
);

-- CreateTable
CREATE TABLE "CarSnapshot" (
    "id" TEXT NOT NULL,
    "brand" TEXT NOT NULL,
    "model" TEXT NOT NULL,
    "generation" TEXT NOT NULL,
    "year" INTEGER NOT NULL,
    "driveType" TEXT NOT NULL,
    "transmissionType" TEXT NOT NULL,
    "engineVolume" DOUBLE PRECISION NOT NULL,
    "fuelType" TEXT NOT NULL,
    "bodyType" TEXT NOT NULL,

    CONSTRAINT "CarSnapshot_pkey" PRIMARY KEY ("id")
);

-- CreateIndex
CREATE UNIQUE INDEX "User_keycloakId_key" ON "User"("keycloakId");

-- CreateIndex
CREATE UNIQUE INDEX "User_email_key" ON "User"("email");

-- CreateIndex
CREATE UNIQUE INDEX "User_username_key" ON "User"("username");

-- CreateIndex
CREATE UNIQUE INDEX "AdSnapshot_carId_key" ON "AdSnapshot"("carId");

-- AddForeignKey
ALTER TABLE "AdSnapshot" ADD CONSTRAINT "AdSnapshot_userId_fkey" FOREIGN KEY ("userId") REFERENCES "User"("id") ON DELETE CASCADE ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE "AdSnapshot" ADD CONSTRAINT "AdSnapshot_carId_fkey" FOREIGN KEY ("carId") REFERENCES "CarSnapshot"("id") ON DELETE SET NULL ON UPDATE CASCADE;
