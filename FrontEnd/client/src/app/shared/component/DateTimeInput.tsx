import { FieldValues, useController, UseControllerProps } from 'react-hook-form';
import { DateTimePicker, DateTimePickerFieldProps } from '@mui/x-date-pickers';
import { TextFieldProps } from '@mui/material';

type OptionalDateTimePickerProps = Partial<Omit<DateTimePickerFieldProps, 'value' | 'onChange'>>;

type Props<T extends FieldValues> = UseControllerProps<T> & OptionalDateTimePickerProps & {
  label?: string;
  textFieldProps?: TextFieldProps;
};

export default function DateTimeInput<T extends FieldValues>(props: Props<T>) {
  const { field, fieldState } = useController({ ...props });
  const { label, textFieldProps, ...rest } = props;

  return (
    <DateTimePicker
      {...rest}
      value={field.value ? new Date(field.value) : null}
      onChange={(value) => field.onChange(value ? new Date(value) : null)}
      sx={{ width: '100%' }}
      slotProps={{
        textField: {
          label,
          onBlur: field.onBlur,
          error: !!fieldState.error,
          helperText: fieldState.error?.message,
          ...textFieldProps
        }
      }}
    />
  );
}
