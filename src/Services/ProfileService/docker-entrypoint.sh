#!/bin/sh
set -e

echo "Running database migrations..."
until npx prisma migrate deploy; do
	echo "Migration failed, retrying in 3s..."
	sleep 3
done

echo "Starting application..."
exec node dist/src/main.js
