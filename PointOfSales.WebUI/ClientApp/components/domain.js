export class Payment {
  constructor(due, paymentType) {
    this.due = due;
    this.tendered = 0.0;
    this.paymentType = paymentType;
    this.change = 0;
  }
}

export const MethodType = {
  Cash: "CASH",
  Card: "CARD"
};
