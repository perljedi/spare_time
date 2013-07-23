package Poker::Dealer;

my(%objects)=();

sub new
{
   my $proto=shift;
   my $class = ref($proto) || $proto;
   return bless({}, $class);
}


sub isFullHouse
{
   my(@cards) = @_;
   my(%values) = ();
   foreach my $card (@cards){
      if(ref($card) && $card->isa('Poker::Card')){
         push @{ $values{$card->getValue()} }, $card;
      }
   }

   if(scalar(keys %values) == 2){
      if(scalar(@{ $values{$cards[0]->getValue()} }) == 2 || scalar(@{ $values{$cards[0]->getValue()} }) == 3){
         if(scalar(@cards) == 5){
            return 1;
         }
      }
   }
   return 0;
}


sub buildHandMetadata
{
   my(@cards) = @_;
   my(%data) = (count=>0);
   foreach my $card (@cards){
      if(ref($card) && $card->isa('Poker::Card')){
         $data{count}++;
         push @{ $data{values}{ $card->getValue() } }, $card;
         push @{ $data{suites}{ $card->getSuite() } }, $card;
      }
   }
   $data{suiteCount} = scalar(keys %{ $data{suites} });
   $data{valueCont} = scalar(keys %{ $data{values} });
   return %data; 
}

return 42;
