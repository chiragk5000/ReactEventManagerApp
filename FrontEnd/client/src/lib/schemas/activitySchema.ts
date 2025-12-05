import { z } from "zod";
import { requiredString } from "../util/util";



export const activitySchema = z.object({
  title: requiredString("Title"),
  description: requiredString("Description"),
  category: requiredString("Category"),

  date: z.coerce
    .date()
    .refine((val) => val instanceof Date && !isNaN(val.getTime()), {
      message: "Date is required",
    }),

  location: z.object({
    venue: requiredString("Venue"),
    city: z.string().optional(),
    latitude: z.coerce.number(),
    longitutde: z.coerce.number(),
  }),
});

export type ActivitySchema = z.infer<typeof activitySchema>;
