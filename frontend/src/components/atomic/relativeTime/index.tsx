import dayjs from "@/config/dayjs";

export interface RelativeTimeProps {
  dateTimeUtc: string;
}

export default function RelativeTime({ dateTimeUtc }: RelativeTimeProps) {
  const dayjsTime = dayjs(dateTimeUtc);

  return (
    <time
      dateTime={dateTimeUtc}
      title={dayjsTime.format("h:mm A, D MMM YYYY")}
      suppressHydrationWarning
    >
      {dayjsTime.fromNow()}
    </time>
  );
}
