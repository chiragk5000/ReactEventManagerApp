import { Button, ButtonProps, LinkProps, styled } from "@mui/material";

type StyledButtonProps = ButtonProps & { component?: React.ElementType } & Partial<LinkProps>;

const StyledButton = styled(Button)<StyledButtonProps>(({ theme }) => ({
  "&.Mui-disabled": {
    backgroundColor: theme.palette.grey[600],
    color: theme.palette.text.disabled,
  },
}));

export default StyledButton;
