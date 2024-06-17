export const getStatusStyles = (status: string) => {
  switch (status) {
    case 'New':
      return {
        border: '1px solid blue',
        color: 'blue',
        borderRadius: 1,
        paddingLeft: 0.5,
        paddingRight: 0.5
      }
    case 'Submitted':
      return {
        border: '1px solid orange',
        color: 'orange',
        borderRadius: 1,
        paddingLeft: 0.5,
        paddingRight: 0.5
      }
    case 'Cancelled':
      return {
        border: '1px solid gray',
        color: 'gray',
        borderRadius: 1,
        paddingLeft: 0.5,
        paddingRight: 0.5
      }
    case 'Approved':
      return {
        border: '1px solid green',
        color: 'green',
        borderRadius: 1,
        paddingLeft: 0.5,
        paddingRight: 0.5
      }
    case 'Rejected':
      return {
        border: '1px solid red',
        color: 'red',
        borderRadius: 1,
        paddingLeft: 0.5,
        paddingRight: 0.5
      }
    default:
      return {}
  }
}
