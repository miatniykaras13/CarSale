import type * as runtime from "@prisma/client/runtime/library";
import type * as Prisma from "../internal/prismaNamespace.js";
export type AdSnapshotModel = runtime.Types.Result.DefaultSelection<Prisma.$AdSnapshotPayload>;
export type AggregateAdSnapshot = {
    _count: AdSnapshotCountAggregateOutputType | null;
    _avg: AdSnapshotAvgAggregateOutputType | null;
    _sum: AdSnapshotSumAggregateOutputType | null;
    _min: AdSnapshotMinAggregateOutputType | null;
    _max: AdSnapshotMaxAggregateOutputType | null;
};
export type AdSnapshotAvgAggregateOutputType = {
    costAmount: number | null;
};
export type AdSnapshotSumAggregateOutputType = {
    costAmount: number | null;
};
export type AdSnapshotMinAggregateOutputType = {
    id: string | null;
    userId: string | null;
    title: string | null;
    description: string | null;
    carId: string | null;
    city: string | null;
    region: string | null;
    costAmount: number | null;
    currencyCode: string | null;
};
export type AdSnapshotMaxAggregateOutputType = {
    id: string | null;
    userId: string | null;
    title: string | null;
    description: string | null;
    carId: string | null;
    city: string | null;
    region: string | null;
    costAmount: number | null;
    currencyCode: string | null;
};
export type AdSnapshotCountAggregateOutputType = {
    id: number;
    userId: number;
    title: number;
    description: number;
    carId: number;
    city: number;
    region: number;
    costAmount: number;
    currencyCode: number;
    _all: number;
};
export type AdSnapshotAvgAggregateInputType = {
    costAmount?: true;
};
export type AdSnapshotSumAggregateInputType = {
    costAmount?: true;
};
export type AdSnapshotMinAggregateInputType = {
    id?: true;
    userId?: true;
    title?: true;
    description?: true;
    carId?: true;
    city?: true;
    region?: true;
    costAmount?: true;
    currencyCode?: true;
};
export type AdSnapshotMaxAggregateInputType = {
    id?: true;
    userId?: true;
    title?: true;
    description?: true;
    carId?: true;
    city?: true;
    region?: true;
    costAmount?: true;
    currencyCode?: true;
};
export type AdSnapshotCountAggregateInputType = {
    id?: true;
    userId?: true;
    title?: true;
    description?: true;
    carId?: true;
    city?: true;
    region?: true;
    costAmount?: true;
    currencyCode?: true;
    _all?: true;
};
export type AdSnapshotAggregateArgs<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = {
    where?: Prisma.AdSnapshotWhereInput;
    orderBy?: Prisma.AdSnapshotOrderByWithRelationInput | Prisma.AdSnapshotOrderByWithRelationInput[];
    cursor?: Prisma.AdSnapshotWhereUniqueInput;
    take?: number;
    skip?: number;
    _count?: true | AdSnapshotCountAggregateInputType;
    _avg?: AdSnapshotAvgAggregateInputType;
    _sum?: AdSnapshotSumAggregateInputType;
    _min?: AdSnapshotMinAggregateInputType;
    _max?: AdSnapshotMaxAggregateInputType;
};
export type GetAdSnapshotAggregateType<T extends AdSnapshotAggregateArgs> = {
    [P in keyof T & keyof AggregateAdSnapshot]: P extends '_count' | 'count' ? T[P] extends true ? number : Prisma.GetScalarType<T[P], AggregateAdSnapshot[P]> : Prisma.GetScalarType<T[P], AggregateAdSnapshot[P]>;
};
export type AdSnapshotGroupByArgs<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = {
    where?: Prisma.AdSnapshotWhereInput;
    orderBy?: Prisma.AdSnapshotOrderByWithAggregationInput | Prisma.AdSnapshotOrderByWithAggregationInput[];
    by: Prisma.AdSnapshotScalarFieldEnum[] | Prisma.AdSnapshotScalarFieldEnum;
    having?: Prisma.AdSnapshotScalarWhereWithAggregatesInput;
    take?: number;
    skip?: number;
    _count?: AdSnapshotCountAggregateInputType | true;
    _avg?: AdSnapshotAvgAggregateInputType;
    _sum?: AdSnapshotSumAggregateInputType;
    _min?: AdSnapshotMinAggregateInputType;
    _max?: AdSnapshotMaxAggregateInputType;
};
export type AdSnapshotGroupByOutputType = {
    id: string;
    userId: string;
    title: string;
    description: string | null;
    carId: string;
    city: string;
    region: string;
    costAmount: number;
    currencyCode: string;
    _count: AdSnapshotCountAggregateOutputType | null;
    _avg: AdSnapshotAvgAggregateOutputType | null;
    _sum: AdSnapshotSumAggregateOutputType | null;
    _min: AdSnapshotMinAggregateOutputType | null;
    _max: AdSnapshotMaxAggregateOutputType | null;
};
type GetAdSnapshotGroupByPayload<T extends AdSnapshotGroupByArgs> = Prisma.PrismaPromise<Array<Prisma.PickEnumerable<AdSnapshotGroupByOutputType, T['by']> & {
    [P in ((keyof T) & (keyof AdSnapshotGroupByOutputType))]: P extends '_count' ? T[P] extends boolean ? number : Prisma.GetScalarType<T[P], AdSnapshotGroupByOutputType[P]> : Prisma.GetScalarType<T[P], AdSnapshotGroupByOutputType[P]>;
}>>;
export type AdSnapshotWhereInput = {
    AND?: Prisma.AdSnapshotWhereInput | Prisma.AdSnapshotWhereInput[];
    OR?: Prisma.AdSnapshotWhereInput[];
    NOT?: Prisma.AdSnapshotWhereInput | Prisma.AdSnapshotWhereInput[];
    id?: Prisma.StringFilter<"AdSnapshot"> | string;
    userId?: Prisma.StringFilter<"AdSnapshot"> | string;
    title?: Prisma.StringFilter<"AdSnapshot"> | string;
    description?: Prisma.StringNullableFilter<"AdSnapshot"> | string | null;
    carId?: Prisma.StringFilter<"AdSnapshot"> | string;
    city?: Prisma.StringFilter<"AdSnapshot"> | string;
    region?: Prisma.StringFilter<"AdSnapshot"> | string;
    costAmount?: Prisma.IntFilter<"AdSnapshot"> | number;
    currencyCode?: Prisma.StringFilter<"AdSnapshot"> | string;
    user?: Prisma.XOR<Prisma.UserScalarRelationFilter, Prisma.UserWhereInput>;
    car?: Prisma.XOR<Prisma.CarSnapshotScalarRelationFilter, Prisma.CarSnapshotWhereInput>;
};
export type AdSnapshotOrderByWithRelationInput = {
    id?: Prisma.SortOrder;
    userId?: Prisma.SortOrder;
    title?: Prisma.SortOrder;
    description?: Prisma.SortOrderInput | Prisma.SortOrder;
    carId?: Prisma.SortOrder;
    city?: Prisma.SortOrder;
    region?: Prisma.SortOrder;
    costAmount?: Prisma.SortOrder;
    currencyCode?: Prisma.SortOrder;
    user?: Prisma.UserOrderByWithRelationInput;
    car?: Prisma.CarSnapshotOrderByWithRelationInput;
};
export type AdSnapshotWhereUniqueInput = Prisma.AtLeast<{
    id?: string;
    carId?: string;
    AND?: Prisma.AdSnapshotWhereInput | Prisma.AdSnapshotWhereInput[];
    OR?: Prisma.AdSnapshotWhereInput[];
    NOT?: Prisma.AdSnapshotWhereInput | Prisma.AdSnapshotWhereInput[];
    userId?: Prisma.StringFilter<"AdSnapshot"> | string;
    title?: Prisma.StringFilter<"AdSnapshot"> | string;
    description?: Prisma.StringNullableFilter<"AdSnapshot"> | string | null;
    city?: Prisma.StringFilter<"AdSnapshot"> | string;
    region?: Prisma.StringFilter<"AdSnapshot"> | string;
    costAmount?: Prisma.IntFilter<"AdSnapshot"> | number;
    currencyCode?: Prisma.StringFilter<"AdSnapshot"> | string;
    user?: Prisma.XOR<Prisma.UserScalarRelationFilter, Prisma.UserWhereInput>;
    car?: Prisma.XOR<Prisma.CarSnapshotScalarRelationFilter, Prisma.CarSnapshotWhereInput>;
}, "id" | "carId">;
export type AdSnapshotOrderByWithAggregationInput = {
    id?: Prisma.SortOrder;
    userId?: Prisma.SortOrder;
    title?: Prisma.SortOrder;
    description?: Prisma.SortOrderInput | Prisma.SortOrder;
    carId?: Prisma.SortOrder;
    city?: Prisma.SortOrder;
    region?: Prisma.SortOrder;
    costAmount?: Prisma.SortOrder;
    currencyCode?: Prisma.SortOrder;
    _count?: Prisma.AdSnapshotCountOrderByAggregateInput;
    _avg?: Prisma.AdSnapshotAvgOrderByAggregateInput;
    _max?: Prisma.AdSnapshotMaxOrderByAggregateInput;
    _min?: Prisma.AdSnapshotMinOrderByAggregateInput;
    _sum?: Prisma.AdSnapshotSumOrderByAggregateInput;
};
export type AdSnapshotScalarWhereWithAggregatesInput = {
    AND?: Prisma.AdSnapshotScalarWhereWithAggregatesInput | Prisma.AdSnapshotScalarWhereWithAggregatesInput[];
    OR?: Prisma.AdSnapshotScalarWhereWithAggregatesInput[];
    NOT?: Prisma.AdSnapshotScalarWhereWithAggregatesInput | Prisma.AdSnapshotScalarWhereWithAggregatesInput[];
    id?: Prisma.StringWithAggregatesFilter<"AdSnapshot"> | string;
    userId?: Prisma.StringWithAggregatesFilter<"AdSnapshot"> | string;
    title?: Prisma.StringWithAggregatesFilter<"AdSnapshot"> | string;
    description?: Prisma.StringNullableWithAggregatesFilter<"AdSnapshot"> | string | null;
    carId?: Prisma.StringWithAggregatesFilter<"AdSnapshot"> | string;
    city?: Prisma.StringWithAggregatesFilter<"AdSnapshot"> | string;
    region?: Prisma.StringWithAggregatesFilter<"AdSnapshot"> | string;
    costAmount?: Prisma.IntWithAggregatesFilter<"AdSnapshot"> | number;
    currencyCode?: Prisma.StringWithAggregatesFilter<"AdSnapshot"> | string;
};
export type AdSnapshotCreateInput = {
    id?: string;
    title: string;
    description?: string | null;
    city: string;
    region: string;
    costAmount: number;
    currencyCode: string;
    user: Prisma.UserCreateNestedOneWithoutAdsInput;
    car: Prisma.CarSnapshotCreateNestedOneWithoutAdInput;
};
export type AdSnapshotUncheckedCreateInput = {
    id?: string;
    userId: string;
    title: string;
    description?: string | null;
    carId: string;
    city: string;
    region: string;
    costAmount: number;
    currencyCode: string;
};
export type AdSnapshotUpdateInput = {
    id?: Prisma.StringFieldUpdateOperationsInput | string;
    title?: Prisma.StringFieldUpdateOperationsInput | string;
    description?: Prisma.NullableStringFieldUpdateOperationsInput | string | null;
    city?: Prisma.StringFieldUpdateOperationsInput | string;
    region?: Prisma.StringFieldUpdateOperationsInput | string;
    costAmount?: Prisma.IntFieldUpdateOperationsInput | number;
    currencyCode?: Prisma.StringFieldUpdateOperationsInput | string;
    user?: Prisma.UserUpdateOneRequiredWithoutAdsNestedInput;
    car?: Prisma.CarSnapshotUpdateOneRequiredWithoutAdNestedInput;
};
export type AdSnapshotUncheckedUpdateInput = {
    id?: Prisma.StringFieldUpdateOperationsInput | string;
    userId?: Prisma.StringFieldUpdateOperationsInput | string;
    title?: Prisma.StringFieldUpdateOperationsInput | string;
    description?: Prisma.NullableStringFieldUpdateOperationsInput | string | null;
    carId?: Prisma.StringFieldUpdateOperationsInput | string;
    city?: Prisma.StringFieldUpdateOperationsInput | string;
    region?: Prisma.StringFieldUpdateOperationsInput | string;
    costAmount?: Prisma.IntFieldUpdateOperationsInput | number;
    currencyCode?: Prisma.StringFieldUpdateOperationsInput | string;
};
export type AdSnapshotCreateManyInput = {
    id?: string;
    userId: string;
    title: string;
    description?: string | null;
    carId: string;
    city: string;
    region: string;
    costAmount: number;
    currencyCode: string;
};
export type AdSnapshotUpdateManyMutationInput = {
    id?: Prisma.StringFieldUpdateOperationsInput | string;
    title?: Prisma.StringFieldUpdateOperationsInput | string;
    description?: Prisma.NullableStringFieldUpdateOperationsInput | string | null;
    city?: Prisma.StringFieldUpdateOperationsInput | string;
    region?: Prisma.StringFieldUpdateOperationsInput | string;
    costAmount?: Prisma.IntFieldUpdateOperationsInput | number;
    currencyCode?: Prisma.StringFieldUpdateOperationsInput | string;
};
export type AdSnapshotUncheckedUpdateManyInput = {
    id?: Prisma.StringFieldUpdateOperationsInput | string;
    userId?: Prisma.StringFieldUpdateOperationsInput | string;
    title?: Prisma.StringFieldUpdateOperationsInput | string;
    description?: Prisma.NullableStringFieldUpdateOperationsInput | string | null;
    carId?: Prisma.StringFieldUpdateOperationsInput | string;
    city?: Prisma.StringFieldUpdateOperationsInput | string;
    region?: Prisma.StringFieldUpdateOperationsInput | string;
    costAmount?: Prisma.IntFieldUpdateOperationsInput | number;
    currencyCode?: Prisma.StringFieldUpdateOperationsInput | string;
};
export type AdSnapshotListRelationFilter = {
    every?: Prisma.AdSnapshotWhereInput;
    some?: Prisma.AdSnapshotWhereInput;
    none?: Prisma.AdSnapshotWhereInput;
};
export type AdSnapshotOrderByRelationAggregateInput = {
    _count?: Prisma.SortOrder;
};
export type AdSnapshotCountOrderByAggregateInput = {
    id?: Prisma.SortOrder;
    userId?: Prisma.SortOrder;
    title?: Prisma.SortOrder;
    description?: Prisma.SortOrder;
    carId?: Prisma.SortOrder;
    city?: Prisma.SortOrder;
    region?: Prisma.SortOrder;
    costAmount?: Prisma.SortOrder;
    currencyCode?: Prisma.SortOrder;
};
export type AdSnapshotAvgOrderByAggregateInput = {
    costAmount?: Prisma.SortOrder;
};
export type AdSnapshotMaxOrderByAggregateInput = {
    id?: Prisma.SortOrder;
    userId?: Prisma.SortOrder;
    title?: Prisma.SortOrder;
    description?: Prisma.SortOrder;
    carId?: Prisma.SortOrder;
    city?: Prisma.SortOrder;
    region?: Prisma.SortOrder;
    costAmount?: Prisma.SortOrder;
    currencyCode?: Prisma.SortOrder;
};
export type AdSnapshotMinOrderByAggregateInput = {
    id?: Prisma.SortOrder;
    userId?: Prisma.SortOrder;
    title?: Prisma.SortOrder;
    description?: Prisma.SortOrder;
    carId?: Prisma.SortOrder;
    city?: Prisma.SortOrder;
    region?: Prisma.SortOrder;
    costAmount?: Prisma.SortOrder;
    currencyCode?: Prisma.SortOrder;
};
export type AdSnapshotSumOrderByAggregateInput = {
    costAmount?: Prisma.SortOrder;
};
export type AdSnapshotNullableScalarRelationFilter = {
    is?: Prisma.AdSnapshotWhereInput | null;
    isNot?: Prisma.AdSnapshotWhereInput | null;
};
export type AdSnapshotCreateNestedManyWithoutUserInput = {
    create?: Prisma.XOR<Prisma.AdSnapshotCreateWithoutUserInput, Prisma.AdSnapshotUncheckedCreateWithoutUserInput> | Prisma.AdSnapshotCreateWithoutUserInput[] | Prisma.AdSnapshotUncheckedCreateWithoutUserInput[];
    connectOrCreate?: Prisma.AdSnapshotCreateOrConnectWithoutUserInput | Prisma.AdSnapshotCreateOrConnectWithoutUserInput[];
    createMany?: Prisma.AdSnapshotCreateManyUserInputEnvelope;
    connect?: Prisma.AdSnapshotWhereUniqueInput | Prisma.AdSnapshotWhereUniqueInput[];
};
export type AdSnapshotUncheckedCreateNestedManyWithoutUserInput = {
    create?: Prisma.XOR<Prisma.AdSnapshotCreateWithoutUserInput, Prisma.AdSnapshotUncheckedCreateWithoutUserInput> | Prisma.AdSnapshotCreateWithoutUserInput[] | Prisma.AdSnapshotUncheckedCreateWithoutUserInput[];
    connectOrCreate?: Prisma.AdSnapshotCreateOrConnectWithoutUserInput | Prisma.AdSnapshotCreateOrConnectWithoutUserInput[];
    createMany?: Prisma.AdSnapshotCreateManyUserInputEnvelope;
    connect?: Prisma.AdSnapshotWhereUniqueInput | Prisma.AdSnapshotWhereUniqueInput[];
};
export type AdSnapshotUpdateManyWithoutUserNestedInput = {
    create?: Prisma.XOR<Prisma.AdSnapshotCreateWithoutUserInput, Prisma.AdSnapshotUncheckedCreateWithoutUserInput> | Prisma.AdSnapshotCreateWithoutUserInput[] | Prisma.AdSnapshotUncheckedCreateWithoutUserInput[];
    connectOrCreate?: Prisma.AdSnapshotCreateOrConnectWithoutUserInput | Prisma.AdSnapshotCreateOrConnectWithoutUserInput[];
    upsert?: Prisma.AdSnapshotUpsertWithWhereUniqueWithoutUserInput | Prisma.AdSnapshotUpsertWithWhereUniqueWithoutUserInput[];
    createMany?: Prisma.AdSnapshotCreateManyUserInputEnvelope;
    set?: Prisma.AdSnapshotWhereUniqueInput | Prisma.AdSnapshotWhereUniqueInput[];
    disconnect?: Prisma.AdSnapshotWhereUniqueInput | Prisma.AdSnapshotWhereUniqueInput[];
    delete?: Prisma.AdSnapshotWhereUniqueInput | Prisma.AdSnapshotWhereUniqueInput[];
    connect?: Prisma.AdSnapshotWhereUniqueInput | Prisma.AdSnapshotWhereUniqueInput[];
    update?: Prisma.AdSnapshotUpdateWithWhereUniqueWithoutUserInput | Prisma.AdSnapshotUpdateWithWhereUniqueWithoutUserInput[];
    updateMany?: Prisma.AdSnapshotUpdateManyWithWhereWithoutUserInput | Prisma.AdSnapshotUpdateManyWithWhereWithoutUserInput[];
    deleteMany?: Prisma.AdSnapshotScalarWhereInput | Prisma.AdSnapshotScalarWhereInput[];
};
export type AdSnapshotUncheckedUpdateManyWithoutUserNestedInput = {
    create?: Prisma.XOR<Prisma.AdSnapshotCreateWithoutUserInput, Prisma.AdSnapshotUncheckedCreateWithoutUserInput> | Prisma.AdSnapshotCreateWithoutUserInput[] | Prisma.AdSnapshotUncheckedCreateWithoutUserInput[];
    connectOrCreate?: Prisma.AdSnapshotCreateOrConnectWithoutUserInput | Prisma.AdSnapshotCreateOrConnectWithoutUserInput[];
    upsert?: Prisma.AdSnapshotUpsertWithWhereUniqueWithoutUserInput | Prisma.AdSnapshotUpsertWithWhereUniqueWithoutUserInput[];
    createMany?: Prisma.AdSnapshotCreateManyUserInputEnvelope;
    set?: Prisma.AdSnapshotWhereUniqueInput | Prisma.AdSnapshotWhereUniqueInput[];
    disconnect?: Prisma.AdSnapshotWhereUniqueInput | Prisma.AdSnapshotWhereUniqueInput[];
    delete?: Prisma.AdSnapshotWhereUniqueInput | Prisma.AdSnapshotWhereUniqueInput[];
    connect?: Prisma.AdSnapshotWhereUniqueInput | Prisma.AdSnapshotWhereUniqueInput[];
    update?: Prisma.AdSnapshotUpdateWithWhereUniqueWithoutUserInput | Prisma.AdSnapshotUpdateWithWhereUniqueWithoutUserInput[];
    updateMany?: Prisma.AdSnapshotUpdateManyWithWhereWithoutUserInput | Prisma.AdSnapshotUpdateManyWithWhereWithoutUserInput[];
    deleteMany?: Prisma.AdSnapshotScalarWhereInput | Prisma.AdSnapshotScalarWhereInput[];
};
export type IntFieldUpdateOperationsInput = {
    set?: number;
    increment?: number;
    decrement?: number;
    multiply?: number;
    divide?: number;
};
export type AdSnapshotCreateNestedOneWithoutCarInput = {
    create?: Prisma.XOR<Prisma.AdSnapshotCreateWithoutCarInput, Prisma.AdSnapshotUncheckedCreateWithoutCarInput>;
    connectOrCreate?: Prisma.AdSnapshotCreateOrConnectWithoutCarInput;
    connect?: Prisma.AdSnapshotWhereUniqueInput;
};
export type AdSnapshotUncheckedCreateNestedOneWithoutCarInput = {
    create?: Prisma.XOR<Prisma.AdSnapshotCreateWithoutCarInput, Prisma.AdSnapshotUncheckedCreateWithoutCarInput>;
    connectOrCreate?: Prisma.AdSnapshotCreateOrConnectWithoutCarInput;
    connect?: Prisma.AdSnapshotWhereUniqueInput;
};
export type AdSnapshotUpdateOneWithoutCarNestedInput = {
    create?: Prisma.XOR<Prisma.AdSnapshotCreateWithoutCarInput, Prisma.AdSnapshotUncheckedCreateWithoutCarInput>;
    connectOrCreate?: Prisma.AdSnapshotCreateOrConnectWithoutCarInput;
    upsert?: Prisma.AdSnapshotUpsertWithoutCarInput;
    disconnect?: Prisma.AdSnapshotWhereInput | boolean;
    delete?: Prisma.AdSnapshotWhereInput | boolean;
    connect?: Prisma.AdSnapshotWhereUniqueInput;
    update?: Prisma.XOR<Prisma.XOR<Prisma.AdSnapshotUpdateToOneWithWhereWithoutCarInput, Prisma.AdSnapshotUpdateWithoutCarInput>, Prisma.AdSnapshotUncheckedUpdateWithoutCarInput>;
};
export type AdSnapshotUncheckedUpdateOneWithoutCarNestedInput = {
    create?: Prisma.XOR<Prisma.AdSnapshotCreateWithoutCarInput, Prisma.AdSnapshotUncheckedCreateWithoutCarInput>;
    connectOrCreate?: Prisma.AdSnapshotCreateOrConnectWithoutCarInput;
    upsert?: Prisma.AdSnapshotUpsertWithoutCarInput;
    disconnect?: Prisma.AdSnapshotWhereInput | boolean;
    delete?: Prisma.AdSnapshotWhereInput | boolean;
    connect?: Prisma.AdSnapshotWhereUniqueInput;
    update?: Prisma.XOR<Prisma.XOR<Prisma.AdSnapshotUpdateToOneWithWhereWithoutCarInput, Prisma.AdSnapshotUpdateWithoutCarInput>, Prisma.AdSnapshotUncheckedUpdateWithoutCarInput>;
};
export type AdSnapshotCreateWithoutUserInput = {
    id?: string;
    title: string;
    description?: string | null;
    city: string;
    region: string;
    costAmount: number;
    currencyCode: string;
    car: Prisma.CarSnapshotCreateNestedOneWithoutAdInput;
};
export type AdSnapshotUncheckedCreateWithoutUserInput = {
    id?: string;
    title: string;
    description?: string | null;
    carId: string;
    city: string;
    region: string;
    costAmount: number;
    currencyCode: string;
};
export type AdSnapshotCreateOrConnectWithoutUserInput = {
    where: Prisma.AdSnapshotWhereUniqueInput;
    create: Prisma.XOR<Prisma.AdSnapshotCreateWithoutUserInput, Prisma.AdSnapshotUncheckedCreateWithoutUserInput>;
};
export type AdSnapshotCreateManyUserInputEnvelope = {
    data: Prisma.AdSnapshotCreateManyUserInput | Prisma.AdSnapshotCreateManyUserInput[];
    skipDuplicates?: boolean;
};
export type AdSnapshotUpsertWithWhereUniqueWithoutUserInput = {
    where: Prisma.AdSnapshotWhereUniqueInput;
    update: Prisma.XOR<Prisma.AdSnapshotUpdateWithoutUserInput, Prisma.AdSnapshotUncheckedUpdateWithoutUserInput>;
    create: Prisma.XOR<Prisma.AdSnapshotCreateWithoutUserInput, Prisma.AdSnapshotUncheckedCreateWithoutUserInput>;
};
export type AdSnapshotUpdateWithWhereUniqueWithoutUserInput = {
    where: Prisma.AdSnapshotWhereUniqueInput;
    data: Prisma.XOR<Prisma.AdSnapshotUpdateWithoutUserInput, Prisma.AdSnapshotUncheckedUpdateWithoutUserInput>;
};
export type AdSnapshotUpdateManyWithWhereWithoutUserInput = {
    where: Prisma.AdSnapshotScalarWhereInput;
    data: Prisma.XOR<Prisma.AdSnapshotUpdateManyMutationInput, Prisma.AdSnapshotUncheckedUpdateManyWithoutUserInput>;
};
export type AdSnapshotScalarWhereInput = {
    AND?: Prisma.AdSnapshotScalarWhereInput | Prisma.AdSnapshotScalarWhereInput[];
    OR?: Prisma.AdSnapshotScalarWhereInput[];
    NOT?: Prisma.AdSnapshotScalarWhereInput | Prisma.AdSnapshotScalarWhereInput[];
    id?: Prisma.StringFilter<"AdSnapshot"> | string;
    userId?: Prisma.StringFilter<"AdSnapshot"> | string;
    title?: Prisma.StringFilter<"AdSnapshot"> | string;
    description?: Prisma.StringNullableFilter<"AdSnapshot"> | string | null;
    carId?: Prisma.StringFilter<"AdSnapshot"> | string;
    city?: Prisma.StringFilter<"AdSnapshot"> | string;
    region?: Prisma.StringFilter<"AdSnapshot"> | string;
    costAmount?: Prisma.IntFilter<"AdSnapshot"> | number;
    currencyCode?: Prisma.StringFilter<"AdSnapshot"> | string;
};
export type AdSnapshotCreateWithoutCarInput = {
    id?: string;
    title: string;
    description?: string | null;
    city: string;
    region: string;
    costAmount: number;
    currencyCode: string;
    user: Prisma.UserCreateNestedOneWithoutAdsInput;
};
export type AdSnapshotUncheckedCreateWithoutCarInput = {
    id?: string;
    userId: string;
    title: string;
    description?: string | null;
    city: string;
    region: string;
    costAmount: number;
    currencyCode: string;
};
export type AdSnapshotCreateOrConnectWithoutCarInput = {
    where: Prisma.AdSnapshotWhereUniqueInput;
    create: Prisma.XOR<Prisma.AdSnapshotCreateWithoutCarInput, Prisma.AdSnapshotUncheckedCreateWithoutCarInput>;
};
export type AdSnapshotUpsertWithoutCarInput = {
    update: Prisma.XOR<Prisma.AdSnapshotUpdateWithoutCarInput, Prisma.AdSnapshotUncheckedUpdateWithoutCarInput>;
    create: Prisma.XOR<Prisma.AdSnapshotCreateWithoutCarInput, Prisma.AdSnapshotUncheckedCreateWithoutCarInput>;
    where?: Prisma.AdSnapshotWhereInput;
};
export type AdSnapshotUpdateToOneWithWhereWithoutCarInput = {
    where?: Prisma.AdSnapshotWhereInput;
    data: Prisma.XOR<Prisma.AdSnapshotUpdateWithoutCarInput, Prisma.AdSnapshotUncheckedUpdateWithoutCarInput>;
};
export type AdSnapshotUpdateWithoutCarInput = {
    id?: Prisma.StringFieldUpdateOperationsInput | string;
    title?: Prisma.StringFieldUpdateOperationsInput | string;
    description?: Prisma.NullableStringFieldUpdateOperationsInput | string | null;
    city?: Prisma.StringFieldUpdateOperationsInput | string;
    region?: Prisma.StringFieldUpdateOperationsInput | string;
    costAmount?: Prisma.IntFieldUpdateOperationsInput | number;
    currencyCode?: Prisma.StringFieldUpdateOperationsInput | string;
    user?: Prisma.UserUpdateOneRequiredWithoutAdsNestedInput;
};
export type AdSnapshotUncheckedUpdateWithoutCarInput = {
    id?: Prisma.StringFieldUpdateOperationsInput | string;
    userId?: Prisma.StringFieldUpdateOperationsInput | string;
    title?: Prisma.StringFieldUpdateOperationsInput | string;
    description?: Prisma.NullableStringFieldUpdateOperationsInput | string | null;
    city?: Prisma.StringFieldUpdateOperationsInput | string;
    region?: Prisma.StringFieldUpdateOperationsInput | string;
    costAmount?: Prisma.IntFieldUpdateOperationsInput | number;
    currencyCode?: Prisma.StringFieldUpdateOperationsInput | string;
};
export type AdSnapshotCreateManyUserInput = {
    id?: string;
    title: string;
    description?: string | null;
    carId: string;
    city: string;
    region: string;
    costAmount: number;
    currencyCode: string;
};
export type AdSnapshotUpdateWithoutUserInput = {
    id?: Prisma.StringFieldUpdateOperationsInput | string;
    title?: Prisma.StringFieldUpdateOperationsInput | string;
    description?: Prisma.NullableStringFieldUpdateOperationsInput | string | null;
    city?: Prisma.StringFieldUpdateOperationsInput | string;
    region?: Prisma.StringFieldUpdateOperationsInput | string;
    costAmount?: Prisma.IntFieldUpdateOperationsInput | number;
    currencyCode?: Prisma.StringFieldUpdateOperationsInput | string;
    car?: Prisma.CarSnapshotUpdateOneRequiredWithoutAdNestedInput;
};
export type AdSnapshotUncheckedUpdateWithoutUserInput = {
    id?: Prisma.StringFieldUpdateOperationsInput | string;
    title?: Prisma.StringFieldUpdateOperationsInput | string;
    description?: Prisma.NullableStringFieldUpdateOperationsInput | string | null;
    carId?: Prisma.StringFieldUpdateOperationsInput | string;
    city?: Prisma.StringFieldUpdateOperationsInput | string;
    region?: Prisma.StringFieldUpdateOperationsInput | string;
    costAmount?: Prisma.IntFieldUpdateOperationsInput | number;
    currencyCode?: Prisma.StringFieldUpdateOperationsInput | string;
};
export type AdSnapshotUncheckedUpdateManyWithoutUserInput = {
    id?: Prisma.StringFieldUpdateOperationsInput | string;
    title?: Prisma.StringFieldUpdateOperationsInput | string;
    description?: Prisma.NullableStringFieldUpdateOperationsInput | string | null;
    carId?: Prisma.StringFieldUpdateOperationsInput | string;
    city?: Prisma.StringFieldUpdateOperationsInput | string;
    region?: Prisma.StringFieldUpdateOperationsInput | string;
    costAmount?: Prisma.IntFieldUpdateOperationsInput | number;
    currencyCode?: Prisma.StringFieldUpdateOperationsInput | string;
};
export type AdSnapshotSelect<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = runtime.Types.Extensions.GetSelect<{
    id?: boolean;
    userId?: boolean;
    title?: boolean;
    description?: boolean;
    carId?: boolean;
    city?: boolean;
    region?: boolean;
    costAmount?: boolean;
    currencyCode?: boolean;
    user?: boolean | Prisma.UserDefaultArgs<ExtArgs>;
    car?: boolean | Prisma.CarSnapshotDefaultArgs<ExtArgs>;
}, ExtArgs["result"]["adSnapshot"]>;
export type AdSnapshotSelectCreateManyAndReturn<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = runtime.Types.Extensions.GetSelect<{
    id?: boolean;
    userId?: boolean;
    title?: boolean;
    description?: boolean;
    carId?: boolean;
    city?: boolean;
    region?: boolean;
    costAmount?: boolean;
    currencyCode?: boolean;
    user?: boolean | Prisma.UserDefaultArgs<ExtArgs>;
    car?: boolean | Prisma.CarSnapshotDefaultArgs<ExtArgs>;
}, ExtArgs["result"]["adSnapshot"]>;
export type AdSnapshotSelectUpdateManyAndReturn<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = runtime.Types.Extensions.GetSelect<{
    id?: boolean;
    userId?: boolean;
    title?: boolean;
    description?: boolean;
    carId?: boolean;
    city?: boolean;
    region?: boolean;
    costAmount?: boolean;
    currencyCode?: boolean;
    user?: boolean | Prisma.UserDefaultArgs<ExtArgs>;
    car?: boolean | Prisma.CarSnapshotDefaultArgs<ExtArgs>;
}, ExtArgs["result"]["adSnapshot"]>;
export type AdSnapshotSelectScalar = {
    id?: boolean;
    userId?: boolean;
    title?: boolean;
    description?: boolean;
    carId?: boolean;
    city?: boolean;
    region?: boolean;
    costAmount?: boolean;
    currencyCode?: boolean;
};
export type AdSnapshotOmit<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = runtime.Types.Extensions.GetOmit<"id" | "userId" | "title" | "description" | "carId" | "city" | "region" | "costAmount" | "currencyCode", ExtArgs["result"]["adSnapshot"]>;
export type AdSnapshotInclude<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = {
    user?: boolean | Prisma.UserDefaultArgs<ExtArgs>;
    car?: boolean | Prisma.CarSnapshotDefaultArgs<ExtArgs>;
};
export type AdSnapshotIncludeCreateManyAndReturn<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = {
    user?: boolean | Prisma.UserDefaultArgs<ExtArgs>;
    car?: boolean | Prisma.CarSnapshotDefaultArgs<ExtArgs>;
};
export type AdSnapshotIncludeUpdateManyAndReturn<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = {
    user?: boolean | Prisma.UserDefaultArgs<ExtArgs>;
    car?: boolean | Prisma.CarSnapshotDefaultArgs<ExtArgs>;
};
export type $AdSnapshotPayload<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = {
    name: "AdSnapshot";
    objects: {
        user: Prisma.$UserPayload<ExtArgs>;
        car: Prisma.$CarSnapshotPayload<ExtArgs>;
    };
    scalars: runtime.Types.Extensions.GetPayloadResult<{
        id: string;
        userId: string;
        title: string;
        description: string | null;
        carId: string;
        city: string;
        region: string;
        costAmount: number;
        currencyCode: string;
    }, ExtArgs["result"]["adSnapshot"]>;
    composites: {};
};
export type AdSnapshotGetPayload<S extends boolean | null | undefined | AdSnapshotDefaultArgs> = runtime.Types.Result.GetResult<Prisma.$AdSnapshotPayload, S>;
export type AdSnapshotCountArgs<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = Omit<AdSnapshotFindManyArgs, 'select' | 'include' | 'distinct' | 'omit'> & {
    select?: AdSnapshotCountAggregateInputType | true;
};
export interface AdSnapshotDelegate<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs, GlobalOmitOptions = {}> {
    [K: symbol]: {
        types: Prisma.TypeMap<ExtArgs>['model']['AdSnapshot'];
        meta: {
            name: 'AdSnapshot';
        };
    };
    findUnique<T extends AdSnapshotFindUniqueArgs>(args: Prisma.SelectSubset<T, AdSnapshotFindUniqueArgs<ExtArgs>>): Prisma.Prisma__AdSnapshotClient<runtime.Types.Result.GetResult<Prisma.$AdSnapshotPayload<ExtArgs>, T, "findUnique", GlobalOmitOptions> | null, null, ExtArgs, GlobalOmitOptions>;
    findUniqueOrThrow<T extends AdSnapshotFindUniqueOrThrowArgs>(args: Prisma.SelectSubset<T, AdSnapshotFindUniqueOrThrowArgs<ExtArgs>>): Prisma.Prisma__AdSnapshotClient<runtime.Types.Result.GetResult<Prisma.$AdSnapshotPayload<ExtArgs>, T, "findUniqueOrThrow", GlobalOmitOptions>, never, ExtArgs, GlobalOmitOptions>;
    findFirst<T extends AdSnapshotFindFirstArgs>(args?: Prisma.SelectSubset<T, AdSnapshotFindFirstArgs<ExtArgs>>): Prisma.Prisma__AdSnapshotClient<runtime.Types.Result.GetResult<Prisma.$AdSnapshotPayload<ExtArgs>, T, "findFirst", GlobalOmitOptions> | null, null, ExtArgs, GlobalOmitOptions>;
    findFirstOrThrow<T extends AdSnapshotFindFirstOrThrowArgs>(args?: Prisma.SelectSubset<T, AdSnapshotFindFirstOrThrowArgs<ExtArgs>>): Prisma.Prisma__AdSnapshotClient<runtime.Types.Result.GetResult<Prisma.$AdSnapshotPayload<ExtArgs>, T, "findFirstOrThrow", GlobalOmitOptions>, never, ExtArgs, GlobalOmitOptions>;
    findMany<T extends AdSnapshotFindManyArgs>(args?: Prisma.SelectSubset<T, AdSnapshotFindManyArgs<ExtArgs>>): Prisma.PrismaPromise<runtime.Types.Result.GetResult<Prisma.$AdSnapshotPayload<ExtArgs>, T, "findMany", GlobalOmitOptions>>;
    create<T extends AdSnapshotCreateArgs>(args: Prisma.SelectSubset<T, AdSnapshotCreateArgs<ExtArgs>>): Prisma.Prisma__AdSnapshotClient<runtime.Types.Result.GetResult<Prisma.$AdSnapshotPayload<ExtArgs>, T, "create", GlobalOmitOptions>, never, ExtArgs, GlobalOmitOptions>;
    createMany<T extends AdSnapshotCreateManyArgs>(args?: Prisma.SelectSubset<T, AdSnapshotCreateManyArgs<ExtArgs>>): Prisma.PrismaPromise<Prisma.BatchPayload>;
    createManyAndReturn<T extends AdSnapshotCreateManyAndReturnArgs>(args?: Prisma.SelectSubset<T, AdSnapshotCreateManyAndReturnArgs<ExtArgs>>): Prisma.PrismaPromise<runtime.Types.Result.GetResult<Prisma.$AdSnapshotPayload<ExtArgs>, T, "createManyAndReturn", GlobalOmitOptions>>;
    delete<T extends AdSnapshotDeleteArgs>(args: Prisma.SelectSubset<T, AdSnapshotDeleteArgs<ExtArgs>>): Prisma.Prisma__AdSnapshotClient<runtime.Types.Result.GetResult<Prisma.$AdSnapshotPayload<ExtArgs>, T, "delete", GlobalOmitOptions>, never, ExtArgs, GlobalOmitOptions>;
    update<T extends AdSnapshotUpdateArgs>(args: Prisma.SelectSubset<T, AdSnapshotUpdateArgs<ExtArgs>>): Prisma.Prisma__AdSnapshotClient<runtime.Types.Result.GetResult<Prisma.$AdSnapshotPayload<ExtArgs>, T, "update", GlobalOmitOptions>, never, ExtArgs, GlobalOmitOptions>;
    deleteMany<T extends AdSnapshotDeleteManyArgs>(args?: Prisma.SelectSubset<T, AdSnapshotDeleteManyArgs<ExtArgs>>): Prisma.PrismaPromise<Prisma.BatchPayload>;
    updateMany<T extends AdSnapshotUpdateManyArgs>(args: Prisma.SelectSubset<T, AdSnapshotUpdateManyArgs<ExtArgs>>): Prisma.PrismaPromise<Prisma.BatchPayload>;
    updateManyAndReturn<T extends AdSnapshotUpdateManyAndReturnArgs>(args: Prisma.SelectSubset<T, AdSnapshotUpdateManyAndReturnArgs<ExtArgs>>): Prisma.PrismaPromise<runtime.Types.Result.GetResult<Prisma.$AdSnapshotPayload<ExtArgs>, T, "updateManyAndReturn", GlobalOmitOptions>>;
    upsert<T extends AdSnapshotUpsertArgs>(args: Prisma.SelectSubset<T, AdSnapshotUpsertArgs<ExtArgs>>): Prisma.Prisma__AdSnapshotClient<runtime.Types.Result.GetResult<Prisma.$AdSnapshotPayload<ExtArgs>, T, "upsert", GlobalOmitOptions>, never, ExtArgs, GlobalOmitOptions>;
    count<T extends AdSnapshotCountArgs>(args?: Prisma.Subset<T, AdSnapshotCountArgs>): Prisma.PrismaPromise<T extends runtime.Types.Utils.Record<'select', any> ? T['select'] extends true ? number : Prisma.GetScalarType<T['select'], AdSnapshotCountAggregateOutputType> : number>;
    aggregate<T extends AdSnapshotAggregateArgs>(args: Prisma.Subset<T, AdSnapshotAggregateArgs>): Prisma.PrismaPromise<GetAdSnapshotAggregateType<T>>;
    groupBy<T extends AdSnapshotGroupByArgs, HasSelectOrTake extends Prisma.Or<Prisma.Extends<'skip', Prisma.Keys<T>>, Prisma.Extends<'take', Prisma.Keys<T>>>, OrderByArg extends Prisma.True extends HasSelectOrTake ? {
        orderBy: AdSnapshotGroupByArgs['orderBy'];
    } : {
        orderBy?: AdSnapshotGroupByArgs['orderBy'];
    }, OrderFields extends Prisma.ExcludeUnderscoreKeys<Prisma.Keys<Prisma.MaybeTupleToUnion<T['orderBy']>>>, ByFields extends Prisma.MaybeTupleToUnion<T['by']>, ByValid extends Prisma.Has<ByFields, OrderFields>, HavingFields extends Prisma.GetHavingFields<T['having']>, HavingValid extends Prisma.Has<ByFields, HavingFields>, ByEmpty extends T['by'] extends never[] ? Prisma.True : Prisma.False, InputErrors extends ByEmpty extends Prisma.True ? `Error: "by" must not be empty.` : HavingValid extends Prisma.False ? {
        [P in HavingFields]: P extends ByFields ? never : P extends string ? `Error: Field "${P}" used in "having" needs to be provided in "by".` : [
            Error,
            'Field ',
            P,
            ` in "having" needs to be provided in "by"`
        ];
    }[HavingFields] : 'take' extends Prisma.Keys<T> ? 'orderBy' extends Prisma.Keys<T> ? ByValid extends Prisma.True ? {} : {
        [P in OrderFields]: P extends ByFields ? never : `Error: Field "${P}" in "orderBy" needs to be provided in "by"`;
    }[OrderFields] : 'Error: If you provide "take", you also need to provide "orderBy"' : 'skip' extends Prisma.Keys<T> ? 'orderBy' extends Prisma.Keys<T> ? ByValid extends Prisma.True ? {} : {
        [P in OrderFields]: P extends ByFields ? never : `Error: Field "${P}" in "orderBy" needs to be provided in "by"`;
    }[OrderFields] : 'Error: If you provide "skip", you also need to provide "orderBy"' : ByValid extends Prisma.True ? {} : {
        [P in OrderFields]: P extends ByFields ? never : `Error: Field "${P}" in "orderBy" needs to be provided in "by"`;
    }[OrderFields]>(args: Prisma.SubsetIntersection<T, AdSnapshotGroupByArgs, OrderByArg> & InputErrors): {} extends InputErrors ? GetAdSnapshotGroupByPayload<T> : Prisma.PrismaPromise<InputErrors>;
    readonly fields: AdSnapshotFieldRefs;
}
export interface Prisma__AdSnapshotClient<T, Null = never, ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs, GlobalOmitOptions = {}> extends Prisma.PrismaPromise<T> {
    readonly [Symbol.toStringTag]: "PrismaPromise";
    user<T extends Prisma.UserDefaultArgs<ExtArgs> = {}>(args?: Prisma.Subset<T, Prisma.UserDefaultArgs<ExtArgs>>): Prisma.Prisma__UserClient<runtime.Types.Result.GetResult<Prisma.$UserPayload<ExtArgs>, T, "findUniqueOrThrow", GlobalOmitOptions> | Null, Null, ExtArgs, GlobalOmitOptions>;
    car<T extends Prisma.CarSnapshotDefaultArgs<ExtArgs> = {}>(args?: Prisma.Subset<T, Prisma.CarSnapshotDefaultArgs<ExtArgs>>): Prisma.Prisma__CarSnapshotClient<runtime.Types.Result.GetResult<Prisma.$CarSnapshotPayload<ExtArgs>, T, "findUniqueOrThrow", GlobalOmitOptions> | Null, Null, ExtArgs, GlobalOmitOptions>;
    then<TResult1 = T, TResult2 = never>(onfulfilled?: ((value: T) => TResult1 | PromiseLike<TResult1>) | undefined | null, onrejected?: ((reason: any) => TResult2 | PromiseLike<TResult2>) | undefined | null): runtime.Types.Utils.JsPromise<TResult1 | TResult2>;
    catch<TResult = never>(onrejected?: ((reason: any) => TResult | PromiseLike<TResult>) | undefined | null): runtime.Types.Utils.JsPromise<T | TResult>;
    finally(onfinally?: (() => void) | undefined | null): runtime.Types.Utils.JsPromise<T>;
}
export interface AdSnapshotFieldRefs {
    readonly id: Prisma.FieldRef<"AdSnapshot", 'String'>;
    readonly userId: Prisma.FieldRef<"AdSnapshot", 'String'>;
    readonly title: Prisma.FieldRef<"AdSnapshot", 'String'>;
    readonly description: Prisma.FieldRef<"AdSnapshot", 'String'>;
    readonly carId: Prisma.FieldRef<"AdSnapshot", 'String'>;
    readonly city: Prisma.FieldRef<"AdSnapshot", 'String'>;
    readonly region: Prisma.FieldRef<"AdSnapshot", 'String'>;
    readonly costAmount: Prisma.FieldRef<"AdSnapshot", 'Int'>;
    readonly currencyCode: Prisma.FieldRef<"AdSnapshot", 'String'>;
}
export type AdSnapshotFindUniqueArgs<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = {
    select?: Prisma.AdSnapshotSelect<ExtArgs> | null;
    omit?: Prisma.AdSnapshotOmit<ExtArgs> | null;
    include?: Prisma.AdSnapshotInclude<ExtArgs> | null;
    where: Prisma.AdSnapshotWhereUniqueInput;
};
export type AdSnapshotFindUniqueOrThrowArgs<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = {
    select?: Prisma.AdSnapshotSelect<ExtArgs> | null;
    omit?: Prisma.AdSnapshotOmit<ExtArgs> | null;
    include?: Prisma.AdSnapshotInclude<ExtArgs> | null;
    where: Prisma.AdSnapshotWhereUniqueInput;
};
export type AdSnapshotFindFirstArgs<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = {
    select?: Prisma.AdSnapshotSelect<ExtArgs> | null;
    omit?: Prisma.AdSnapshotOmit<ExtArgs> | null;
    include?: Prisma.AdSnapshotInclude<ExtArgs> | null;
    where?: Prisma.AdSnapshotWhereInput;
    orderBy?: Prisma.AdSnapshotOrderByWithRelationInput | Prisma.AdSnapshotOrderByWithRelationInput[];
    cursor?: Prisma.AdSnapshotWhereUniqueInput;
    take?: number;
    skip?: number;
    distinct?: Prisma.AdSnapshotScalarFieldEnum | Prisma.AdSnapshotScalarFieldEnum[];
};
export type AdSnapshotFindFirstOrThrowArgs<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = {
    select?: Prisma.AdSnapshotSelect<ExtArgs> | null;
    omit?: Prisma.AdSnapshotOmit<ExtArgs> | null;
    include?: Prisma.AdSnapshotInclude<ExtArgs> | null;
    where?: Prisma.AdSnapshotWhereInput;
    orderBy?: Prisma.AdSnapshotOrderByWithRelationInput | Prisma.AdSnapshotOrderByWithRelationInput[];
    cursor?: Prisma.AdSnapshotWhereUniqueInput;
    take?: number;
    skip?: number;
    distinct?: Prisma.AdSnapshotScalarFieldEnum | Prisma.AdSnapshotScalarFieldEnum[];
};
export type AdSnapshotFindManyArgs<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = {
    select?: Prisma.AdSnapshotSelect<ExtArgs> | null;
    omit?: Prisma.AdSnapshotOmit<ExtArgs> | null;
    include?: Prisma.AdSnapshotInclude<ExtArgs> | null;
    where?: Prisma.AdSnapshotWhereInput;
    orderBy?: Prisma.AdSnapshotOrderByWithRelationInput | Prisma.AdSnapshotOrderByWithRelationInput[];
    cursor?: Prisma.AdSnapshotWhereUniqueInput;
    take?: number;
    skip?: number;
    distinct?: Prisma.AdSnapshotScalarFieldEnum | Prisma.AdSnapshotScalarFieldEnum[];
};
export type AdSnapshotCreateArgs<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = {
    select?: Prisma.AdSnapshotSelect<ExtArgs> | null;
    omit?: Prisma.AdSnapshotOmit<ExtArgs> | null;
    include?: Prisma.AdSnapshotInclude<ExtArgs> | null;
    data: Prisma.XOR<Prisma.AdSnapshotCreateInput, Prisma.AdSnapshotUncheckedCreateInput>;
};
export type AdSnapshotCreateManyArgs<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = {
    data: Prisma.AdSnapshotCreateManyInput | Prisma.AdSnapshotCreateManyInput[];
    skipDuplicates?: boolean;
};
export type AdSnapshotCreateManyAndReturnArgs<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = {
    select?: Prisma.AdSnapshotSelectCreateManyAndReturn<ExtArgs> | null;
    omit?: Prisma.AdSnapshotOmit<ExtArgs> | null;
    data: Prisma.AdSnapshotCreateManyInput | Prisma.AdSnapshotCreateManyInput[];
    skipDuplicates?: boolean;
    include?: Prisma.AdSnapshotIncludeCreateManyAndReturn<ExtArgs> | null;
};
export type AdSnapshotUpdateArgs<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = {
    select?: Prisma.AdSnapshotSelect<ExtArgs> | null;
    omit?: Prisma.AdSnapshotOmit<ExtArgs> | null;
    include?: Prisma.AdSnapshotInclude<ExtArgs> | null;
    data: Prisma.XOR<Prisma.AdSnapshotUpdateInput, Prisma.AdSnapshotUncheckedUpdateInput>;
    where: Prisma.AdSnapshotWhereUniqueInput;
};
export type AdSnapshotUpdateManyArgs<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = {
    data: Prisma.XOR<Prisma.AdSnapshotUpdateManyMutationInput, Prisma.AdSnapshotUncheckedUpdateManyInput>;
    where?: Prisma.AdSnapshotWhereInput;
    limit?: number;
};
export type AdSnapshotUpdateManyAndReturnArgs<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = {
    select?: Prisma.AdSnapshotSelectUpdateManyAndReturn<ExtArgs> | null;
    omit?: Prisma.AdSnapshotOmit<ExtArgs> | null;
    data: Prisma.XOR<Prisma.AdSnapshotUpdateManyMutationInput, Prisma.AdSnapshotUncheckedUpdateManyInput>;
    where?: Prisma.AdSnapshotWhereInput;
    limit?: number;
    include?: Prisma.AdSnapshotIncludeUpdateManyAndReturn<ExtArgs> | null;
};
export type AdSnapshotUpsertArgs<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = {
    select?: Prisma.AdSnapshotSelect<ExtArgs> | null;
    omit?: Prisma.AdSnapshotOmit<ExtArgs> | null;
    include?: Prisma.AdSnapshotInclude<ExtArgs> | null;
    where: Prisma.AdSnapshotWhereUniqueInput;
    create: Prisma.XOR<Prisma.AdSnapshotCreateInput, Prisma.AdSnapshotUncheckedCreateInput>;
    update: Prisma.XOR<Prisma.AdSnapshotUpdateInput, Prisma.AdSnapshotUncheckedUpdateInput>;
};
export type AdSnapshotDeleteArgs<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = {
    select?: Prisma.AdSnapshotSelect<ExtArgs> | null;
    omit?: Prisma.AdSnapshotOmit<ExtArgs> | null;
    include?: Prisma.AdSnapshotInclude<ExtArgs> | null;
    where: Prisma.AdSnapshotWhereUniqueInput;
};
export type AdSnapshotDeleteManyArgs<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = {
    where?: Prisma.AdSnapshotWhereInput;
    limit?: number;
};
export type AdSnapshotDefaultArgs<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = {
    select?: Prisma.AdSnapshotSelect<ExtArgs> | null;
    omit?: Prisma.AdSnapshotOmit<ExtArgs> | null;
    include?: Prisma.AdSnapshotInclude<ExtArgs> | null;
};
export {};
