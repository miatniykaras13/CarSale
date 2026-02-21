import { CarSnapshot } from "./carSnapshot.model"
import { LocationSnapshot } from "./locationSnapshot.model"
import { Money } from "./money.model"

export class AdSnapshot {
  id: string
  
  title: string
  description?: string
  car: CarSnapshot
  location: LocationSnapshot
  price: Money
}