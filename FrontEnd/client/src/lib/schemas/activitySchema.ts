
import {z} from 'zod';

// this is function to validate string
const requiredString= (fieldname:string) => z.
    string({required_error:`${fieldname} is required`})
    .min(1,{message:`${fieldname} is required`})

export const activitySchema = z.object({
    title: requiredString('Title'),
    description: requiredString('Description'),
    category: requiredString('Category'),
    date: z.coerce.date({
        message:'Date is required'
    }),
    location:z.object({
        venue:requiredString('Venue'),
        city:z.string().optional(),
        latitude:z.coerce.number(),
        longitutde:z.coerce.number()
    })
})
 
export type ActivitySchema = z.infer<typeof activitySchema>;
