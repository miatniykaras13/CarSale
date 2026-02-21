import { v6 as uuid } from 'uuid'
import { AdSnapshot } from './adSnapshot.model'

export class User {
  id: string
  email: string
  password: string

  keycloakId: string

  ads: AdSnapshot[]
}