import React from 'react';
import { FieldRenderProps } from 'react-final-form';
import { DateTimePicker } from 'react-widgets';
import { FormFieldProps, Form, Label } from 'semantic-ui-react';

interface IProps extends FieldRenderProps<Date, HTMLElement>, FormFieldProps {}

const DateInput: React.FC<IProps> = ({
  input,
  width,
  type,
  placeholder,
  date = false,
  time = false,
  meta: { touched, error },
}) => {
  return (
    <Form.Field error={touched && !!error} type={type} width={width}>
      <DateTimePicker
        placeholder={placeholder}
        value={input.value || null}
        onChange={input.onChange}
        date={date}
        time={time}
        onBlur={input.onBlur}
        onKeyDown={(e) => e.preventDefault()}
      />
      {touched && !!error && (
        <Label basic color='red'>
          {error}
        </Label>
      )}
    </Form.Field>
  );
};

export default DateInput;
