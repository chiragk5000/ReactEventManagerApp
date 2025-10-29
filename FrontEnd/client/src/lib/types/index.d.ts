
type Activity = {
  id: string
  title: string
  date: Date
  description: string
  category: string
  isCancelled: boolean
  city: string
  venue: string
  latitude: number
  longitutde: number
}


type User={
  id:string
  email:string
  displayName:string
  bio:string
  imageUrl?:string
}

type LocationIQSuggestion = {
  place_id: string
  osm_type: string
  osm_id: string
  licence: string
  lat: number
  lon: number
  boundingbox: string[]
  class: string
  display_name: string
  display_place: string
  display_address: string
  address: LocationIQAddress

}


type LocationIQAddress = {
  name: string
  house_number: string
  road: string
  suburb?: string
  town?:string
  village?:string
  city?: string
  county: string
  state: string
  postcode: string
  country: string
  country_code: string
  neighbourhood?: string

}


