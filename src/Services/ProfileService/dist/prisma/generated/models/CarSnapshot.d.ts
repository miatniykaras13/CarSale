import type * as runtime from "@prisma/client/runtime/library";
import type * as Prisma from "../internal/prismaNamespace.js";
export type CarSnapshotModel = runtime.Types.Result.DefaultSelection<Prisma.$CarSnapshotPayload>;
export type AggregateCarSnapshot = {
    _count: CarSnapshotCountAggregateOutputType | null;
    _avg: CarSnapshotAvgAggregateOutputType | null;
    _sum: CarSnapshotSumAggregateOutputType | null;
    _min: CarSnapshotMinAggregateOutputType | null;
    _max: CarSnapshotMaxAggregateOutputType | null;
};
export type CarSnapshotAvgAggregateOutputType = {
    year: number | null;
    engineVolume: number | null;
};
export type CarSnapshotSumAggregateOutputType = {
    year: number | null;
    engineVolume: number | null;
};
export type CarSnapshotMinAggregateOutputType = {
    id: string | null;
    brand: string | null;
    model: string | null;
    generation: string | null;
    year: number | null;
    driveType: string | null;
    transmissionType: string | null;
    engineVolume: number | null;
    fuelType: string | null;
    bodyType: string | null;
};
export type CarSnapshotMaxAggregateOutputType = {
    id: string | null;
    brand: string | null;
    model: string | null;
    generation: string | null;
    year: number | null;
    driveType: string | null;
    transmissionType: string | null;
    engineVolume: number | null;
    fuelType: string | null;
    bodyType: string | null;
};
export type CarSnapshotCountAggregateOutputType = {
    id: number;
    brand: number;
    model: number;
    generation: number;
    year: number;
    driveType: number;
    transmissionType: number;
    engineVolume: number;
    fuelType: number;
    bodyType: number;
    _all: number;
};
export type CarSnapshotAvgAggregateInputType = {
    year?: true;
    engineVolume?: true;
};
export type CarSnapshotSumAggregateInputType = {
    year?: true;
    engineVolume?: true;
};
export type CarSnapshotMinAggregateInputType = {
    id?: true;
    brand?: true;
    model?: true;
    generation?: true;
    year?: true;
    driveType?: true;
    transmissionType?: true;
    engineVolume?: true;
    fuelType?: true;
    bodyType?: true;
};
export type CarSnapshotMaxAggregateInputType = {
    id?: true;
    brand?: true;
    model?: true;
    generation?: true;
    year?: true;
    driveType?: true;
    transmissionType?: true;
    engineVolume?: true;
    fuelType?: true;
    bodyType?: true;
};
export type CarSnapshotCountAggregateInputType = {
    id?: true;
    brand?: true;
    model?: true;
    generation?: true;
    year?: true;
    driveType?: true;
    transmissionType?: true;
    engineVolume?: true;
    fuelType?: true;
    bodyType?: true;
    _all?: true;
};
export type CarSnapshotAggregateArgs<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = {
    where?: Prisma.CarSnapshotWhereInput;
    orderBy?: Prisma.CarSnapshotOrderByWithRelationInput | Prisma.CarSnapshotOrderByWithRelationInput[];
    cursor?: Prisma.CarSnapshotWhereUniqueInput;
    take?: number;
    skip?: number;
    _count?: true | CarSnapshotCountAggregateInputType;
    _avg?: CarSnapshotAvgAggregateInputType;
    _sum?: CarSnapshotSumAggregateInputType;
    _min?: CarSnapshotMinAggregateInputType;
    _max?: CarSnapshotMaxAggregateInputType;
};
export type GetCarSnapshotAggregateType<T extends CarSnapshotAggregateArgs> = {
    [P in keyof T & keyof AggregateCarSnapshot]: P extends '_count' | 'count' ? T[P] extends true ? number : Prisma.GetScalarType<T[P], AggregateCarSnapshot[P]> : Prisma.GetScalarType<T[P], AggregateCarSnapshot[P]>;
};
export type CarSnapshotGroupByArgs<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = {
    where?: Prisma.CarSnapshotWhereInput;
    orderBy?: Prisma.CarSnapshotOrderByWithAggregationInput | Prisma.CarSnapshotOrderByWithAggregationInput[];
    by: Prisma.CarSnapshotScalarFieldEnum[] | Prisma.CarSnapshotScalarFieldEnum;
    having?: Prisma.CarSnapshotScalarWhereWithAggregatesInput;
    take?: number;
    skip?: number;
    _count?: CarSnapshotCountAggregateInputType | true;
    _avg?: CarSnapshotAvgAggregateInputType;
    _sum?: CarSnapshotSumAggregateInputType;
    _min?: CarSnapshotMinAggregateInputType;
    _max?: CarSnapshotMaxAggregateInputType;
};
export type CarSnapshotGroupByOutputType = {
    id: string;
    brand: string;
    model: string;
    generation: string;
    year: number;
    driveType: string;
    transmissionType: string;
    engineVolume: number;
    fuelType: string;
    bodyType: string;
    _count: CarSnapshotCountAggregateOutputType | null;
    _avg: CarSnapshotAvgAggregateOutputType | null;
    _sum: CarSnapshotSumAggregateOutputType | null;
    _min: CarSnapshotMinAggregateOutputType | null;
    _max: CarSnapshotMaxAggregateOutputType | null;
};
type GetCarSnapshotGroupByPayload<T extends CarSnapshotGroupByArgs> = Prisma.PrismaPromise<Array<Prisma.PickEnumerable<CarSnapshotGroupByOutputType, T['by']> & {
    [P in ((keyof T) & (keyof CarSnapshotGroupByOutputType))]: P extends '_count' ? T[P] extends boolean ? number : Prisma.GetScalarType<T[P], CarSnapshotGroupByOutputType[P]> : Prisma.GetScalarType<T[P], CarSnapshotGroupByOutputType[P]>;
}>>;
export type CarSnapshotWhereInput = {
    AND?: Prisma.CarSnapshotWhereInput | Prisma.CarSnapshotWhereInput[];
    OR?: Prisma.CarSnapshotWhereInput[];
    NOT?: Prisma.CarSnapshotWhereInput | Prisma.CarSnapshotWhereInput[];
    id?: Prisma.StringFilter<"CarSnapshot"> | string;
    brand?: Prisma.StringFilter<"CarSnapshot"> | string;
    model?: Prisma.StringFilter<"CarSnapshot"> | string;
    generation?: Prisma.StringFilter<"CarSnapshot"> | string;
    year?: Prisma.IntFilter<"CarSnapshot"> | number;
    driveType?: Prisma.StringFilter<"CarSnapshot"> | string;
    transmissionType?: Prisma.StringFilter<"CarSnapshot"> | string;
    engineVolume?: Prisma.FloatFilter<"CarSnapshot"> | number;
    fuelType?: Prisma.StringFilter<"CarSnapshot"> | string;
    bodyType?: Prisma.StringFilter<"CarSnapshot"> | string;
    ad?: Prisma.XOR<Prisma.AdSnapshotNullableScalarRelationFilter, Prisma.AdSnapshotWhereInput> | null;
};
export type CarSnapshotOrderByWithRelationInput = {
    id?: Prisma.SortOrder;
    brand?: Prisma.SortOrder;
    model?: Prisma.SortOrder;
    generation?: Prisma.SortOrder;
    year?: Prisma.SortOrder;
    driveType?: Prisma.SortOrder;
    transmissionType?: Prisma.SortOrder;
    engineVolume?: Prisma.SortOrder;
    fuelType?: Prisma.SortOrder;
    bodyType?: Prisma.SortOrder;
    ad?: Prisma.AdSnapshotOrderByWithRelationInput;
};
export type CarSnapshotWhereUniqueInput = Prisma.AtLeast<{
    id?: string;
    AND?: Prisma.CarSnapshotWhereInput | Prisma.CarSnapshotWhereInput[];
    OR?: Prisma.CarSnapshotWhereInput[];
    NOT?: Prisma.CarSnapshotWhereInput | Prisma.CarSnapshotWhereInput[];
    brand?: Prisma.StringFilter<"CarSnapshot"> | string;
    model?: Prisma.StringFilter<"CarSnapshot"> | string;
    generation?: Prisma.StringFilter<"CarSnapshot"> | string;
    year?: Prisma.IntFilter<"CarSnapshot"> | number;
    driveType?: Prisma.StringFilter<"CarSnapshot"> | string;
    transmissionType?: Prisma.StringFilter<"CarSnapshot"> | string;
    engineVolume?: Prisma.FloatFilter<"CarSnapshot"> | number;
    fuelType?: Prisma.StringFilter<"CarSnapshot"> | string;
    bodyType?: Prisma.StringFilter<"CarSnapshot"> | string;
    ad?: Prisma.XOR<Prisma.AdSnapshotNullableScalarRelationFilter, Prisma.AdSnapshotWhereInput> | null;
}, "id">;
export type CarSnapshotOrderByWithAggregationInput = {
    id?: Prisma.SortOrder;
    brand?: Prisma.SortOrder;
    model?: Prisma.SortOrder;
    generation?: Prisma.SortOrder;
    year?: Prisma.SortOrder;
    driveType?: Prisma.SortOrder;
    transmissionType?: Prisma.SortOrder;
    engineVolume?: Prisma.SortOrder;
    fuelType?: Prisma.SortOrder;
    bodyType?: Prisma.SortOrder;
    _count?: Prisma.CarSnapshotCountOrderByAggregateInput;
    _avg?: Prisma.CarSnapshotAvgOrderByAggregateInput;
    _max?: Prisma.CarSnapshotMaxOrderByAggregateInput;
    _min?: Prisma.CarSnapshotMinOrderByAggregateInput;
    _sum?: Prisma.CarSnapshotSumOrderByAggregateInput;
};
export type CarSnapshotScalarWhereWithAggregatesInput = {
    AND?: Prisma.CarSnapshotScalarWhereWithAggregatesInput | Prisma.CarSnapshotScalarWhereWithAggregatesInput[];
    OR?: Prisma.CarSnapshotScalarWhereWithAggregatesInput[];
    NOT?: Prisma.CarSnapshotScalarWhereWithAggregatesInput | Prisma.CarSnapshotScalarWhereWithAggregatesInput[];
    id?: Prisma.StringWithAggregatesFilter<"CarSnapshot"> | string;
    brand?: Prisma.StringWithAggregatesFilter<"CarSnapshot"> | string;
    model?: Prisma.StringWithAggregatesFilter<"CarSnapshot"> | string;
    generation?: Prisma.StringWithAggregatesFilter<"CarSnapshot"> | string;
    year?: Prisma.IntWithAggregatesFilter<"CarSnapshot"> | number;
    driveType?: Prisma.StringWithAggregatesFilter<"CarSnapshot"> | string;
    transmissionType?: Prisma.StringWithAggregatesFilter<"CarSnapshot"> | string;
    engineVolume?: Prisma.FloatWithAggregatesFilter<"CarSnapshot"> | number;
    fuelType?: Prisma.StringWithAggregatesFilter<"CarSnapshot"> | string;
    bodyType?: Prisma.StringWithAggregatesFilter<"CarSnapshot"> | string;
};
export type CarSnapshotCreateInput = {
    id?: string;
    brand: string;
    model: string;
    generation: string;
    year: number;
    driveType: string;
    transmissionType: string;
    engineVolume: number;
    fuelType: string;
    bodyType: string;
    ad?: Prisma.AdSnapshotCreateNestedOneWithoutCarInput;
};
export type CarSnapshotUncheckedCreateInput = {
    id?: string;
    brand: string;
    model: string;
    generation: string;
    year: number;
    driveType: string;
    transmissionType: string;
    engineVolume: number;
    fuelType: string;
    bodyType: string;
    ad?: Prisma.AdSnapshotUncheckedCreateNestedOneWithoutCarInput;
};
export type CarSnapshotUpdateInput = {
    id?: Prisma.StringFieldUpdateOperationsInput | string;
    brand?: Prisma.StringFieldUpdateOperationsInput | string;
    model?: Prisma.StringFieldUpdateOperationsInput | string;
    generation?: Prisma.StringFieldUpdateOperationsInput | string;
    year?: Prisma.IntFieldUpdateOperationsInput | number;
    driveType?: Prisma.StringFieldUpdateOperationsInput | string;
    transmissionType?: Prisma.StringFieldUpdateOperationsInput | string;
    engineVolume?: Prisma.FloatFieldUpdateOperationsInput | number;
    fuelType?: Prisma.StringFieldUpdateOperationsInput | string;
    bodyType?: Prisma.StringFieldUpdateOperationsInput | string;
    ad?: Prisma.AdSnapshotUpdateOneWithoutCarNestedInput;
};
export type CarSnapshotUncheckedUpdateInput = {
    id?: Prisma.StringFieldUpdateOperationsInput | string;
    brand?: Prisma.StringFieldUpdateOperationsInput | string;
    model?: Prisma.StringFieldUpdateOperationsInput | string;
    generation?: Prisma.StringFieldUpdateOperationsInput | string;
    year?: Prisma.IntFieldUpdateOperationsInput | number;
    driveType?: Prisma.StringFieldUpdateOperationsInput | string;
    transmissionType?: Prisma.StringFieldUpdateOperationsInput | string;
    engineVolume?: Prisma.FloatFieldUpdateOperationsInput | number;
    fuelType?: Prisma.StringFieldUpdateOperationsInput | string;
    bodyType?: Prisma.StringFieldUpdateOperationsInput | string;
    ad?: Prisma.AdSnapshotUncheckedUpdateOneWithoutCarNestedInput;
};
export type CarSnapshotCreateManyInput = {
    id?: string;
    brand: string;
    model: string;
    generation: string;
    year: number;
    driveType: string;
    transmissionType: string;
    engineVolume: number;
    fuelType: string;
    bodyType: string;
};
export type CarSnapshotUpdateManyMutationInput = {
    id?: Prisma.StringFieldUpdateOperationsInput | string;
    brand?: Prisma.StringFieldUpdateOperationsInput | string;
    model?: Prisma.StringFieldUpdateOperationsInput | string;
    generation?: Prisma.StringFieldUpdateOperationsInput | string;
    year?: Prisma.IntFieldUpdateOperationsInput | number;
    driveType?: Prisma.StringFieldUpdateOperationsInput | string;
    transmissionType?: Prisma.StringFieldUpdateOperationsInput | string;
    engineVolume?: Prisma.FloatFieldUpdateOperationsInput | number;
    fuelType?: Prisma.StringFieldUpdateOperationsInput | string;
    bodyType?: Prisma.StringFieldUpdateOperationsInput | string;
};
export type CarSnapshotUncheckedUpdateManyInput = {
    id?: Prisma.StringFieldUpdateOperationsInput | string;
    brand?: Prisma.StringFieldUpdateOperationsInput | string;
    model?: Prisma.StringFieldUpdateOperationsInput | string;
    generation?: Prisma.StringFieldUpdateOperationsInput | string;
    year?: Prisma.IntFieldUpdateOperationsInput | number;
    driveType?: Prisma.StringFieldUpdateOperationsInput | string;
    transmissionType?: Prisma.StringFieldUpdateOperationsInput | string;
    engineVolume?: Prisma.FloatFieldUpdateOperationsInput | number;
    fuelType?: Prisma.StringFieldUpdateOperationsInput | string;
    bodyType?: Prisma.StringFieldUpdateOperationsInput | string;
};
export type CarSnapshotNullableScalarRelationFilter = {
    is?: Prisma.CarSnapshotWhereInput | null;
    isNot?: Prisma.CarSnapshotWhereInput | null;
};
export type CarSnapshotCountOrderByAggregateInput = {
    id?: Prisma.SortOrder;
    brand?: Prisma.SortOrder;
    model?: Prisma.SortOrder;
    generation?: Prisma.SortOrder;
    year?: Prisma.SortOrder;
    driveType?: Prisma.SortOrder;
    transmissionType?: Prisma.SortOrder;
    engineVolume?: Prisma.SortOrder;
    fuelType?: Prisma.SortOrder;
    bodyType?: Prisma.SortOrder;
};
export type CarSnapshotAvgOrderByAggregateInput = {
    year?: Prisma.SortOrder;
    engineVolume?: Prisma.SortOrder;
};
export type CarSnapshotMaxOrderByAggregateInput = {
    id?: Prisma.SortOrder;
    brand?: Prisma.SortOrder;
    model?: Prisma.SortOrder;
    generation?: Prisma.SortOrder;
    year?: Prisma.SortOrder;
    driveType?: Prisma.SortOrder;
    transmissionType?: Prisma.SortOrder;
    engineVolume?: Prisma.SortOrder;
    fuelType?: Prisma.SortOrder;
    bodyType?: Prisma.SortOrder;
};
export type CarSnapshotMinOrderByAggregateInput = {
    id?: Prisma.SortOrder;
    brand?: Prisma.SortOrder;
    model?: Prisma.SortOrder;
    generation?: Prisma.SortOrder;
    year?: Prisma.SortOrder;
    driveType?: Prisma.SortOrder;
    transmissionType?: Prisma.SortOrder;
    engineVolume?: Prisma.SortOrder;
    fuelType?: Prisma.SortOrder;
    bodyType?: Prisma.SortOrder;
};
export type CarSnapshotSumOrderByAggregateInput = {
    year?: Prisma.SortOrder;
    engineVolume?: Prisma.SortOrder;
};
export type CarSnapshotCreateNestedOneWithoutAdInput = {
    create?: Prisma.XOR<Prisma.CarSnapshotCreateWithoutAdInput, Prisma.CarSnapshotUncheckedCreateWithoutAdInput>;
    connectOrCreate?: Prisma.CarSnapshotCreateOrConnectWithoutAdInput;
    connect?: Prisma.CarSnapshotWhereUniqueInput;
};
export type CarSnapshotUpdateOneWithoutAdNestedInput = {
    create?: Prisma.XOR<Prisma.CarSnapshotCreateWithoutAdInput, Prisma.CarSnapshotUncheckedCreateWithoutAdInput>;
    connectOrCreate?: Prisma.CarSnapshotCreateOrConnectWithoutAdInput;
    upsert?: Prisma.CarSnapshotUpsertWithoutAdInput;
    disconnect?: Prisma.CarSnapshotWhereInput | boolean;
    delete?: Prisma.CarSnapshotWhereInput | boolean;
    connect?: Prisma.CarSnapshotWhereUniqueInput;
    update?: Prisma.XOR<Prisma.XOR<Prisma.CarSnapshotUpdateToOneWithWhereWithoutAdInput, Prisma.CarSnapshotUpdateWithoutAdInput>, Prisma.CarSnapshotUncheckedUpdateWithoutAdInput>;
};
export type IntFieldUpdateOperationsInput = {
    set?: number;
    increment?: number;
    decrement?: number;
    multiply?: number;
    divide?: number;
};
export type FloatFieldUpdateOperationsInput = {
    set?: number;
    increment?: number;
    decrement?: number;
    multiply?: number;
    divide?: number;
};
export type CarSnapshotCreateWithoutAdInput = {
    id?: string;
    brand: string;
    model: string;
    generation: string;
    year: number;
    driveType: string;
    transmissionType: string;
    engineVolume: number;
    fuelType: string;
    bodyType: string;
};
export type CarSnapshotUncheckedCreateWithoutAdInput = {
    id?: string;
    brand: string;
    model: string;
    generation: string;
    year: number;
    driveType: string;
    transmissionType: string;
    engineVolume: number;
    fuelType: string;
    bodyType: string;
};
export type CarSnapshotCreateOrConnectWithoutAdInput = {
    where: Prisma.CarSnapshotWhereUniqueInput;
    create: Prisma.XOR<Prisma.CarSnapshotCreateWithoutAdInput, Prisma.CarSnapshotUncheckedCreateWithoutAdInput>;
};
export type CarSnapshotUpsertWithoutAdInput = {
    update: Prisma.XOR<Prisma.CarSnapshotUpdateWithoutAdInput, Prisma.CarSnapshotUncheckedUpdateWithoutAdInput>;
    create: Prisma.XOR<Prisma.CarSnapshotCreateWithoutAdInput, Prisma.CarSnapshotUncheckedCreateWithoutAdInput>;
    where?: Prisma.CarSnapshotWhereInput;
};
export type CarSnapshotUpdateToOneWithWhereWithoutAdInput = {
    where?: Prisma.CarSnapshotWhereInput;
    data: Prisma.XOR<Prisma.CarSnapshotUpdateWithoutAdInput, Prisma.CarSnapshotUncheckedUpdateWithoutAdInput>;
};
export type CarSnapshotUpdateWithoutAdInput = {
    id?: Prisma.StringFieldUpdateOperationsInput | string;
    brand?: Prisma.StringFieldUpdateOperationsInput | string;
    model?: Prisma.StringFieldUpdateOperationsInput | string;
    generation?: Prisma.StringFieldUpdateOperationsInput | string;
    year?: Prisma.IntFieldUpdateOperationsInput | number;
    driveType?: Prisma.StringFieldUpdateOperationsInput | string;
    transmissionType?: Prisma.StringFieldUpdateOperationsInput | string;
    engineVolume?: Prisma.FloatFieldUpdateOperationsInput | number;
    fuelType?: Prisma.StringFieldUpdateOperationsInput | string;
    bodyType?: Prisma.StringFieldUpdateOperationsInput | string;
};
export type CarSnapshotUncheckedUpdateWithoutAdInput = {
    id?: Prisma.StringFieldUpdateOperationsInput | string;
    brand?: Prisma.StringFieldUpdateOperationsInput | string;
    model?: Prisma.StringFieldUpdateOperationsInput | string;
    generation?: Prisma.StringFieldUpdateOperationsInput | string;
    year?: Prisma.IntFieldUpdateOperationsInput | number;
    driveType?: Prisma.StringFieldUpdateOperationsInput | string;
    transmissionType?: Prisma.StringFieldUpdateOperationsInput | string;
    engineVolume?: Prisma.FloatFieldUpdateOperationsInput | number;
    fuelType?: Prisma.StringFieldUpdateOperationsInput | string;
    bodyType?: Prisma.StringFieldUpdateOperationsInput | string;
};
export type CarSnapshotSelect<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = runtime.Types.Extensions.GetSelect<{
    id?: boolean;
    brand?: boolean;
    model?: boolean;
    generation?: boolean;
    year?: boolean;
    driveType?: boolean;
    transmissionType?: boolean;
    engineVolume?: boolean;
    fuelType?: boolean;
    bodyType?: boolean;
    ad?: boolean | Prisma.CarSnapshot$adArgs<ExtArgs>;
}, ExtArgs["result"]["carSnapshot"]>;
export type CarSnapshotSelectCreateManyAndReturn<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = runtime.Types.Extensions.GetSelect<{
    id?: boolean;
    brand?: boolean;
    model?: boolean;
    generation?: boolean;
    year?: boolean;
    driveType?: boolean;
    transmissionType?: boolean;
    engineVolume?: boolean;
    fuelType?: boolean;
    bodyType?: boolean;
}, ExtArgs["result"]["carSnapshot"]>;
export type CarSnapshotSelectUpdateManyAndReturn<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = runtime.Types.Extensions.GetSelect<{
    id?: boolean;
    brand?: boolean;
    model?: boolean;
    generation?: boolean;
    year?: boolean;
    driveType?: boolean;
    transmissionType?: boolean;
    engineVolume?: boolean;
    fuelType?: boolean;
    bodyType?: boolean;
}, ExtArgs["result"]["carSnapshot"]>;
export type CarSnapshotSelectScalar = {
    id?: boolean;
    brand?: boolean;
    model?: boolean;
    generation?: boolean;
    year?: boolean;
    driveType?: boolean;
    transmissionType?: boolean;
    engineVolume?: boolean;
    fuelType?: boolean;
    bodyType?: boolean;
};
export type CarSnapshotOmit<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = runtime.Types.Extensions.GetOmit<"id" | "brand" | "model" | "generation" | "year" | "driveType" | "transmissionType" | "engineVolume" | "fuelType" | "bodyType", ExtArgs["result"]["carSnapshot"]>;
export type CarSnapshotInclude<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = {
    ad?: boolean | Prisma.CarSnapshot$adArgs<ExtArgs>;
};
export type CarSnapshotIncludeCreateManyAndReturn<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = {};
export type CarSnapshotIncludeUpdateManyAndReturn<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = {};
export type $CarSnapshotPayload<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = {
    name: "CarSnapshot";
    objects: {
        ad: Prisma.$AdSnapshotPayload<ExtArgs> | null;
    };
    scalars: runtime.Types.Extensions.GetPayloadResult<{
        id: string;
        brand: string;
        model: string;
        generation: string;
        year: number;
        driveType: string;
        transmissionType: string;
        engineVolume: number;
        fuelType: string;
        bodyType: string;
    }, ExtArgs["result"]["carSnapshot"]>;
    composites: {};
};
export type CarSnapshotGetPayload<S extends boolean | null | undefined | CarSnapshotDefaultArgs> = runtime.Types.Result.GetResult<Prisma.$CarSnapshotPayload, S>;
export type CarSnapshotCountArgs<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = Omit<CarSnapshotFindManyArgs, 'select' | 'include' | 'distinct' | 'omit'> & {
    select?: CarSnapshotCountAggregateInputType | true;
};
export interface CarSnapshotDelegate<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs, GlobalOmitOptions = {}> {
    [K: symbol]: {
        types: Prisma.TypeMap<ExtArgs>['model']['CarSnapshot'];
        meta: {
            name: 'CarSnapshot';
        };
    };
    findUnique<T extends CarSnapshotFindUniqueArgs>(args: Prisma.SelectSubset<T, CarSnapshotFindUniqueArgs<ExtArgs>>): Prisma.Prisma__CarSnapshotClient<runtime.Types.Result.GetResult<Prisma.$CarSnapshotPayload<ExtArgs>, T, "findUnique", GlobalOmitOptions> | null, null, ExtArgs, GlobalOmitOptions>;
    findUniqueOrThrow<T extends CarSnapshotFindUniqueOrThrowArgs>(args: Prisma.SelectSubset<T, CarSnapshotFindUniqueOrThrowArgs<ExtArgs>>): Prisma.Prisma__CarSnapshotClient<runtime.Types.Result.GetResult<Prisma.$CarSnapshotPayload<ExtArgs>, T, "findUniqueOrThrow", GlobalOmitOptions>, never, ExtArgs, GlobalOmitOptions>;
    findFirst<T extends CarSnapshotFindFirstArgs>(args?: Prisma.SelectSubset<T, CarSnapshotFindFirstArgs<ExtArgs>>): Prisma.Prisma__CarSnapshotClient<runtime.Types.Result.GetResult<Prisma.$CarSnapshotPayload<ExtArgs>, T, "findFirst", GlobalOmitOptions> | null, null, ExtArgs, GlobalOmitOptions>;
    findFirstOrThrow<T extends CarSnapshotFindFirstOrThrowArgs>(args?: Prisma.SelectSubset<T, CarSnapshotFindFirstOrThrowArgs<ExtArgs>>): Prisma.Prisma__CarSnapshotClient<runtime.Types.Result.GetResult<Prisma.$CarSnapshotPayload<ExtArgs>, T, "findFirstOrThrow", GlobalOmitOptions>, never, ExtArgs, GlobalOmitOptions>;
    findMany<T extends CarSnapshotFindManyArgs>(args?: Prisma.SelectSubset<T, CarSnapshotFindManyArgs<ExtArgs>>): Prisma.PrismaPromise<runtime.Types.Result.GetResult<Prisma.$CarSnapshotPayload<ExtArgs>, T, "findMany", GlobalOmitOptions>>;
    create<T extends CarSnapshotCreateArgs>(args: Prisma.SelectSubset<T, CarSnapshotCreateArgs<ExtArgs>>): Prisma.Prisma__CarSnapshotClient<runtime.Types.Result.GetResult<Prisma.$CarSnapshotPayload<ExtArgs>, T, "create", GlobalOmitOptions>, never, ExtArgs, GlobalOmitOptions>;
    createMany<T extends CarSnapshotCreateManyArgs>(args?: Prisma.SelectSubset<T, CarSnapshotCreateManyArgs<ExtArgs>>): Prisma.PrismaPromise<Prisma.BatchPayload>;
    createManyAndReturn<T extends CarSnapshotCreateManyAndReturnArgs>(args?: Prisma.SelectSubset<T, CarSnapshotCreateManyAndReturnArgs<ExtArgs>>): Prisma.PrismaPromise<runtime.Types.Result.GetResult<Prisma.$CarSnapshotPayload<ExtArgs>, T, "createManyAndReturn", GlobalOmitOptions>>;
    delete<T extends CarSnapshotDeleteArgs>(args: Prisma.SelectSubset<T, CarSnapshotDeleteArgs<ExtArgs>>): Prisma.Prisma__CarSnapshotClient<runtime.Types.Result.GetResult<Prisma.$CarSnapshotPayload<ExtArgs>, T, "delete", GlobalOmitOptions>, never, ExtArgs, GlobalOmitOptions>;
    update<T extends CarSnapshotUpdateArgs>(args: Prisma.SelectSubset<T, CarSnapshotUpdateArgs<ExtArgs>>): Prisma.Prisma__CarSnapshotClient<runtime.Types.Result.GetResult<Prisma.$CarSnapshotPayload<ExtArgs>, T, "update", GlobalOmitOptions>, never, ExtArgs, GlobalOmitOptions>;
    deleteMany<T extends CarSnapshotDeleteManyArgs>(args?: Prisma.SelectSubset<T, CarSnapshotDeleteManyArgs<ExtArgs>>): Prisma.PrismaPromise<Prisma.BatchPayload>;
    updateMany<T extends CarSnapshotUpdateManyArgs>(args: Prisma.SelectSubset<T, CarSnapshotUpdateManyArgs<ExtArgs>>): Prisma.PrismaPromise<Prisma.BatchPayload>;
    updateManyAndReturn<T extends CarSnapshotUpdateManyAndReturnArgs>(args: Prisma.SelectSubset<T, CarSnapshotUpdateManyAndReturnArgs<ExtArgs>>): Prisma.PrismaPromise<runtime.Types.Result.GetResult<Prisma.$CarSnapshotPayload<ExtArgs>, T, "updateManyAndReturn", GlobalOmitOptions>>;
    upsert<T extends CarSnapshotUpsertArgs>(args: Prisma.SelectSubset<T, CarSnapshotUpsertArgs<ExtArgs>>): Prisma.Prisma__CarSnapshotClient<runtime.Types.Result.GetResult<Prisma.$CarSnapshotPayload<ExtArgs>, T, "upsert", GlobalOmitOptions>, never, ExtArgs, GlobalOmitOptions>;
    count<T extends CarSnapshotCountArgs>(args?: Prisma.Subset<T, CarSnapshotCountArgs>): Prisma.PrismaPromise<T extends runtime.Types.Utils.Record<'select', any> ? T['select'] extends true ? number : Prisma.GetScalarType<T['select'], CarSnapshotCountAggregateOutputType> : number>;
    aggregate<T extends CarSnapshotAggregateArgs>(args: Prisma.Subset<T, CarSnapshotAggregateArgs>): Prisma.PrismaPromise<GetCarSnapshotAggregateType<T>>;
    groupBy<T extends CarSnapshotGroupByArgs, HasSelectOrTake extends Prisma.Or<Prisma.Extends<'skip', Prisma.Keys<T>>, Prisma.Extends<'take', Prisma.Keys<T>>>, OrderByArg extends Prisma.True extends HasSelectOrTake ? {
        orderBy: CarSnapshotGroupByArgs['orderBy'];
    } : {
        orderBy?: CarSnapshotGroupByArgs['orderBy'];
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
    }[OrderFields]>(args: Prisma.SubsetIntersection<T, CarSnapshotGroupByArgs, OrderByArg> & InputErrors): {} extends InputErrors ? GetCarSnapshotGroupByPayload<T> : Prisma.PrismaPromise<InputErrors>;
    readonly fields: CarSnapshotFieldRefs;
}
export interface Prisma__CarSnapshotClient<T, Null = never, ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs, GlobalOmitOptions = {}> extends Prisma.PrismaPromise<T> {
    readonly [Symbol.toStringTag]: "PrismaPromise";
    ad<T extends Prisma.CarSnapshot$adArgs<ExtArgs> = {}>(args?: Prisma.Subset<T, Prisma.CarSnapshot$adArgs<ExtArgs>>): Prisma.Prisma__AdSnapshotClient<runtime.Types.Result.GetResult<Prisma.$AdSnapshotPayload<ExtArgs>, T, "findUniqueOrThrow", GlobalOmitOptions> | null, null, ExtArgs, GlobalOmitOptions>;
    then<TResult1 = T, TResult2 = never>(onfulfilled?: ((value: T) => TResult1 | PromiseLike<TResult1>) | undefined | null, onrejected?: ((reason: any) => TResult2 | PromiseLike<TResult2>) | undefined | null): runtime.Types.Utils.JsPromise<TResult1 | TResult2>;
    catch<TResult = never>(onrejected?: ((reason: any) => TResult | PromiseLike<TResult>) | undefined | null): runtime.Types.Utils.JsPromise<T | TResult>;
    finally(onfinally?: (() => void) | undefined | null): runtime.Types.Utils.JsPromise<T>;
}
export interface CarSnapshotFieldRefs {
    readonly id: Prisma.FieldRef<"CarSnapshot", 'String'>;
    readonly brand: Prisma.FieldRef<"CarSnapshot", 'String'>;
    readonly model: Prisma.FieldRef<"CarSnapshot", 'String'>;
    readonly generation: Prisma.FieldRef<"CarSnapshot", 'String'>;
    readonly year: Prisma.FieldRef<"CarSnapshot", 'Int'>;
    readonly driveType: Prisma.FieldRef<"CarSnapshot", 'String'>;
    readonly transmissionType: Prisma.FieldRef<"CarSnapshot", 'String'>;
    readonly engineVolume: Prisma.FieldRef<"CarSnapshot", 'Float'>;
    readonly fuelType: Prisma.FieldRef<"CarSnapshot", 'String'>;
    readonly bodyType: Prisma.FieldRef<"CarSnapshot", 'String'>;
}
export type CarSnapshotFindUniqueArgs<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = {
    select?: Prisma.CarSnapshotSelect<ExtArgs> | null;
    omit?: Prisma.CarSnapshotOmit<ExtArgs> | null;
    include?: Prisma.CarSnapshotInclude<ExtArgs> | null;
    where: Prisma.CarSnapshotWhereUniqueInput;
};
export type CarSnapshotFindUniqueOrThrowArgs<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = {
    select?: Prisma.CarSnapshotSelect<ExtArgs> | null;
    omit?: Prisma.CarSnapshotOmit<ExtArgs> | null;
    include?: Prisma.CarSnapshotInclude<ExtArgs> | null;
    where: Prisma.CarSnapshotWhereUniqueInput;
};
export type CarSnapshotFindFirstArgs<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = {
    select?: Prisma.CarSnapshotSelect<ExtArgs> | null;
    omit?: Prisma.CarSnapshotOmit<ExtArgs> | null;
    include?: Prisma.CarSnapshotInclude<ExtArgs> | null;
    where?: Prisma.CarSnapshotWhereInput;
    orderBy?: Prisma.CarSnapshotOrderByWithRelationInput | Prisma.CarSnapshotOrderByWithRelationInput[];
    cursor?: Prisma.CarSnapshotWhereUniqueInput;
    take?: number;
    skip?: number;
    distinct?: Prisma.CarSnapshotScalarFieldEnum | Prisma.CarSnapshotScalarFieldEnum[];
};
export type CarSnapshotFindFirstOrThrowArgs<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = {
    select?: Prisma.CarSnapshotSelect<ExtArgs> | null;
    omit?: Prisma.CarSnapshotOmit<ExtArgs> | null;
    include?: Prisma.CarSnapshotInclude<ExtArgs> | null;
    where?: Prisma.CarSnapshotWhereInput;
    orderBy?: Prisma.CarSnapshotOrderByWithRelationInput | Prisma.CarSnapshotOrderByWithRelationInput[];
    cursor?: Prisma.CarSnapshotWhereUniqueInput;
    take?: number;
    skip?: number;
    distinct?: Prisma.CarSnapshotScalarFieldEnum | Prisma.CarSnapshotScalarFieldEnum[];
};
export type CarSnapshotFindManyArgs<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = {
    select?: Prisma.CarSnapshotSelect<ExtArgs> | null;
    omit?: Prisma.CarSnapshotOmit<ExtArgs> | null;
    include?: Prisma.CarSnapshotInclude<ExtArgs> | null;
    where?: Prisma.CarSnapshotWhereInput;
    orderBy?: Prisma.CarSnapshotOrderByWithRelationInput | Prisma.CarSnapshotOrderByWithRelationInput[];
    cursor?: Prisma.CarSnapshotWhereUniqueInput;
    take?: number;
    skip?: number;
    distinct?: Prisma.CarSnapshotScalarFieldEnum | Prisma.CarSnapshotScalarFieldEnum[];
};
export type CarSnapshotCreateArgs<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = {
    select?: Prisma.CarSnapshotSelect<ExtArgs> | null;
    omit?: Prisma.CarSnapshotOmit<ExtArgs> | null;
    include?: Prisma.CarSnapshotInclude<ExtArgs> | null;
    data: Prisma.XOR<Prisma.CarSnapshotCreateInput, Prisma.CarSnapshotUncheckedCreateInput>;
};
export type CarSnapshotCreateManyArgs<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = {
    data: Prisma.CarSnapshotCreateManyInput | Prisma.CarSnapshotCreateManyInput[];
    skipDuplicates?: boolean;
};
export type CarSnapshotCreateManyAndReturnArgs<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = {
    select?: Prisma.CarSnapshotSelectCreateManyAndReturn<ExtArgs> | null;
    omit?: Prisma.CarSnapshotOmit<ExtArgs> | null;
    data: Prisma.CarSnapshotCreateManyInput | Prisma.CarSnapshotCreateManyInput[];
    skipDuplicates?: boolean;
};
export type CarSnapshotUpdateArgs<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = {
    select?: Prisma.CarSnapshotSelect<ExtArgs> | null;
    omit?: Prisma.CarSnapshotOmit<ExtArgs> | null;
    include?: Prisma.CarSnapshotInclude<ExtArgs> | null;
    data: Prisma.XOR<Prisma.CarSnapshotUpdateInput, Prisma.CarSnapshotUncheckedUpdateInput>;
    where: Prisma.CarSnapshotWhereUniqueInput;
};
export type CarSnapshotUpdateManyArgs<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = {
    data: Prisma.XOR<Prisma.CarSnapshotUpdateManyMutationInput, Prisma.CarSnapshotUncheckedUpdateManyInput>;
    where?: Prisma.CarSnapshotWhereInput;
    limit?: number;
};
export type CarSnapshotUpdateManyAndReturnArgs<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = {
    select?: Prisma.CarSnapshotSelectUpdateManyAndReturn<ExtArgs> | null;
    omit?: Prisma.CarSnapshotOmit<ExtArgs> | null;
    data: Prisma.XOR<Prisma.CarSnapshotUpdateManyMutationInput, Prisma.CarSnapshotUncheckedUpdateManyInput>;
    where?: Prisma.CarSnapshotWhereInput;
    limit?: number;
};
export type CarSnapshotUpsertArgs<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = {
    select?: Prisma.CarSnapshotSelect<ExtArgs> | null;
    omit?: Prisma.CarSnapshotOmit<ExtArgs> | null;
    include?: Prisma.CarSnapshotInclude<ExtArgs> | null;
    where: Prisma.CarSnapshotWhereUniqueInput;
    create: Prisma.XOR<Prisma.CarSnapshotCreateInput, Prisma.CarSnapshotUncheckedCreateInput>;
    update: Prisma.XOR<Prisma.CarSnapshotUpdateInput, Prisma.CarSnapshotUncheckedUpdateInput>;
};
export type CarSnapshotDeleteArgs<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = {
    select?: Prisma.CarSnapshotSelect<ExtArgs> | null;
    omit?: Prisma.CarSnapshotOmit<ExtArgs> | null;
    include?: Prisma.CarSnapshotInclude<ExtArgs> | null;
    where: Prisma.CarSnapshotWhereUniqueInput;
};
export type CarSnapshotDeleteManyArgs<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = {
    where?: Prisma.CarSnapshotWhereInput;
    limit?: number;
};
export type CarSnapshot$adArgs<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = {
    select?: Prisma.AdSnapshotSelect<ExtArgs> | null;
    omit?: Prisma.AdSnapshotOmit<ExtArgs> | null;
    include?: Prisma.AdSnapshotInclude<ExtArgs> | null;
    where?: Prisma.AdSnapshotWhereInput;
};
export type CarSnapshotDefaultArgs<ExtArgs extends runtime.Types.Extensions.InternalArgs = runtime.Types.Extensions.DefaultArgs> = {
    select?: Prisma.CarSnapshotSelect<ExtArgs> | null;
    omit?: Prisma.CarSnapshotOmit<ExtArgs> | null;
    include?: Prisma.CarSnapshotInclude<ExtArgs> | null;
};
export {};
