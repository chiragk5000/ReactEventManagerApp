import { FieldValues,useController ,UseControllerProps } from "react-hook-form"
import { LocationIQSuggestion } from "../../../lib/types";
import { Box, debounce, List, ListItemButton, TextField, Typography } from "@mui/material";
import { useEffect, useMemo, useState } from "react";
import axios from "axios";

type Props<T extends FieldValues> = {label:string} & UseControllerProps<T> 


export default function LocationInput<T extends FieldValues> (props: Props<T>) {
  const {field,fieldState} =useController({...props});
  const[loading,setLoading] = useState(false);
  const [suggestions,setSuggestions]=useState<LocationIQSuggestion[]>([]);
  const[inputValue,setInputValue]=useState(field.value || '');

  useEffect(()=>{
    if(field.value && typeof field.value==='object'){
        setInputValue(field.value.venue || '');
    }
    else{
        setInputValue(field.value || '');
    }
  },[field.value])

  const locationUrl ='https://api.locationiq.com/v1/autocomplete?key=pk.0bbd133d37f013dcd30d90cbc4cc26af&limit=5&dedupe=1&'

  const fetchSuggestions = useMemo(
    ()=> debounce(async (query:string)=>{
        if(!query || query.length < 3){
            setSuggestions([])
            return;
        }
        setLoading(true);
        try{
            const res = await axios.get<LocationIQSuggestion[]>(`${locationUrl}q=${query}`)
                setSuggestions(res.data)
        }   
        catch (error){
            console.log(error);
        }

        finally {
            setLoading(false);
        }

    },500),[locationUrl]
);

const handleChange= async(value:string) => {
    field.onChange(value);
    await fetchSuggestions(value);
}

const handleSelect = (location:LocationIQSuggestion)=>{
    const city = location.address?.city || location.address?.town || location.address?.village;
    const venue = location.display_name;
    const latitude = location.lat;
    const longitutde = location.lon;
    setInputValue(venue);

    field.onChange({ city, venue, latitude: latitude,longitutde:longitutde })
      console.log("Selected Location:", { city, venue, latitude, longitutde });

    setSuggestions([]);
}


   return (
    <Box>
      <TextField
        {...props}
        value={inputValue}
        onChange={e=>handleChange(e.target.value)}
        fullWidth
        variant="outlined"
        error={!!fieldState.error}
        helperText={fieldState.error?.message}
      />

      {loading && <Typography>Loading...</Typography>}

      {suggestions.length > 0 && (
        <List sx={{ border: 1, mt: 1, maxHeight: 200, overflowY: 'auto' }}>
          {suggestions.map((s) => (
            <ListItemButton
              key={s.place_id}
              divider
              onClick={() => handleSelect(s)}
            >
              {s.display_name}
            </ListItemButton>
          ))}
        </List>
      )}
    </Box>
  );
}
