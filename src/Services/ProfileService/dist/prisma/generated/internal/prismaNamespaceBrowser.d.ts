import * as runtime from "@prisma/client/runtime/index-browser";
export type * from '../models.js';
export type * from './prismaNamespace.js';
export declare const Decimal: typeof runtime.Decimal;
export declare const NullTypes: {
    DbNull: (new (secret: never) => typeof runtime.objectEnumValues.instances.DbNull);
    JsonNull: (new (secret: never) => typeof runtime.objectEnumValues.instances.JsonNull);
    AnyNull: (new (secret: never) => typeof runtime.objectEnumValues.instances.AnyNull);
};
export declare const DbNull: {
    "__#private@#private": any;
    _getNamespace(): string;
    _getName(): string;
    toString(): string;
};
export declare const JsonNull: {
    "__#private@#private": any;
    _getNamespace(): string;
    _getName(): string;
    toString(): string;
};
export declare const AnyNull: {
    "__#private@#private": any;
    _getNamespace(): string;
    _getName(): string;
    toString(): string;
};
export declare const ModelName: {
    readonly User: "User";
    readonly AdSnapshot: "AdSnapshot";
    readonly CarSnapshot: "CarSnapshot";
};
export type ModelName = (typeof ModelName)[keyof typeof ModelName];
export declare const TransactionIsolationLevel: {
    readonly ReadUncommitted: "ReadUncommitted";
    readonly ReadCommitted: "ReadCommitted";
    readonly RepeatableRead: "RepeatableRead";
    readonly Serializable: "Serializable";
};
export type TransactionIsolationLevel = (typeof TransactionIsolationLevel)[keyof typeof TransactionIsolationLevel];
export declare const UserScalarFieldEnum: {
    readonly id: "id";
    readonly keycloakId: "keycloakId";
    readonly email: "email";
    readonly username: "username";
    readonly name: "name";
    readonly surname: "surname";
    readonly picture: "picture";
    readonly createdAt: "createdAt";
    readonly updatedAt: "updatedAt";
};
export type UserScalarFieldEnum = (typeof UserScalarFieldEnum)[keyof typeof UserScalarFieldEnum];
export declare const AdSnapshotScalarFieldEnum: {
    readonly id: "id";
    readonly userId: "userId";
    readonly title: "title";
    readonly description: "description";
    readonly carId: "carId";
    readonly city: "city";
    readonly country: "country";
    readonly costAmount: "costAmount";
    readonly currencyCode: "currencyCode";
};
export type AdSnapshotScalarFieldEnum = (typeof AdSnapshotScalarFieldEnum)[keyof typeof AdSnapshotScalarFieldEnum];
export declare const CarSnapshotScalarFieldEnum: {
    readonly id: "id";
    readonly brand: "brand";
    readonly model: "model";
    readonly generation: "generation";
    readonly year: "year";
    readonly driveType: "driveType";
    readonly transmissionType: "transmissionType";
    readonly volume: "volume";
    readonly fuelType: "fuelType";
    readonly bodyType: "bodyType";
};
export type CarSnapshotScalarFieldEnum = (typeof CarSnapshotScalarFieldEnum)[keyof typeof CarSnapshotScalarFieldEnum];
export declare const SortOrder: {
    readonly asc: "asc";
    readonly desc: "desc";
};
export type SortOrder = (typeof SortOrder)[keyof typeof SortOrder];
export declare const QueryMode: {
    readonly default: "default";
    readonly insensitive: "insensitive";
};
export type QueryMode = (typeof QueryMode)[keyof typeof QueryMode];
export declare const NullsOrder: {
    readonly first: "first";
    readonly last: "last";
};
export type NullsOrder = (typeof NullsOrder)[keyof typeof NullsOrder];
