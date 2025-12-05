import { TextField, TextFieldProps } from '@mui/material';
import { FieldValues, useController, UseControllerProps, Control } from 'react-hook-form';

type TextInputProps<T extends FieldValues> = {
  control: Control<T>;
} & UseControllerProps<T> &
  Omit<TextFieldProps, 'name' | 'defaultValue'>;

export default function TextInput<T extends FieldValues>({
  control,
  name,
  ...props
}: TextInputProps<T>) {
  const { field, fieldState } = useController({ name, control });
  
  return (
    <TextField
      {...props}
      {...field}
      value={field.value ?? ''}
      fullWidth
      variant="outlined"
      error={!!fieldState.error}
      helperText={fieldState.error?.message}
    />
  );
}
