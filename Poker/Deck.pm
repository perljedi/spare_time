package Poker::Deck;
use List::Util;
require Poker::Card;
use Carp;

my(%deck_objects)=();
sub new{
   my $proto = shift;
   my $d='deck';
   my $self = bless(\$d, ref($proto) || $proto);
   my(@cards)=();
   my(%master);
   foreach my $suite (qw(c d h s)){
      foreach my $val (2 .. 10, qw(j q k a)){
         my $card = Poker::Card->new($val, $suite);
         $master{$card}=$card;
         push @cards, $card;
      }
   }
   $deck_objects{$self}={cards=>\@cards, discardPile=>[], master=>\%master};
	return $self;
}

sub shuffle{
   my $key = shift;
   my $self = $deck_objects{$key};
   $self->{cards} = [List::Util::shuffle(@{ $self->{cards} })];
   return 1;
}

sub perfectShuffle{
   my $key = shift;
   my $self = $deck_objects{$key};
   my(@fh) = @{ $self->{cards} }[0 .. 25];
   my(@sh) = @{ $self->{cards} }[26 .. 51];
   my(@cards)=();
   foreach my $c (@fh){
      push @cards, $c;
      push @cards, shift @sh;
   }
   $self->{cards}=\@cards;
   return 1;
}

sub customShuffle
{
   my $key = shift;
   my $itterations=shift;
   my $self = $deck_objects{$key};
   my(@cards) = @{ $self->{cards} };
   while($itterations>0){
      my $ws = rand(3);
      if($ws > 2){
         @cards=&_handShuffle(@cards);
      }elsif($ws > 1){
         @cards=&_cutDeck(@cards);
      }else{
         @cards=&_randShuffle(@cards);
      }
      $itterations--;
   }
   $self->{cards}=\@cards;
   return 1;
}

sub getCard{
   my $key = shift;
   my $self = $deck_objects{$key};
   return shift @{ $self->{cards} };
}

sub deckSize{
   my $key = shift;
   my $self = $deck_objects{$key};

   return scalar(@{ $self->{cards} });
}

sub burnCard
{
   my $self = shift;
   $self->discard($self->getCard());
   return 1;
}

sub discard
{
   my $key = shift;
   my $self = $deck_objects{$key};
   my $card = shift;
   if(! defined($card) || ! $card->isa('Poker::Card')){
      croak("Cannot discard invalid card");
   }elsif(! exists($self->{master}{$card})){
      croak('Cannot discard a card which does not belong to this deck');
   }else{
      push @{ $self->{discardPile} }, $card;
      return 1;
   }
}

sub burnSize
{
   my $key = shift;
   my $self = $deck_objects{$key};
   return scalar(@{ $self->{discardPile} });
}

sub _handShuffle
{
   my(@cards)=@_;
   my(@return)=();
   my $point=int(rand(26));
   $point+=13;
   my(@fh) = @cards[0 .. $point];
   $point++;
   my(@sh) = @cards[$point .. 51];
   while(scalar(@return) < 52){
      push @return, shift @fh if(scalar(@fh));
      push @return, shift @sh if(scalar(@sh));
   }
   return @return;
}

sub _cutDeck
{
   my(@cards)=@_;
   my $si=int(rand(8))+1;
   return (@cards[$si .. 51],@cards[0 .. --$si]);
}

sub _randShuffle
{
   my(@cards)=@_;
   my(@return)=();
   while(scalar(@cards) > 8){
      my $stack = int(rand(6))+2;
      push @return, splice(@cards, -$stack);
   }
   push @return, @cards;
   return @return;
}

return 42;
