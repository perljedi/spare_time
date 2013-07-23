use Test::More;
use Test::Exception;
use Regexp::Common;

use_ok('Poker::Deck');

can_ok(Poker::Deck, qw(new shuffle getCard deckSize burnCard discard burnSize));

my $deck = new_ok(Poker::Deck);

like($deck->deckSize(), qr/$RE{num}{int}/, 'Deck size returns a number');

isa_ok($deck->getCard(), 'Poker::Card');

subtest 'Deck size decreases as cards are Dealt' => sub{
   my $deck = Poker::Deck->new();
   cmp_ok($deck->deckSize(), '==', 52, 'deck starts with 52 cards');
   $deck->getCard();
   cmp_ok($deck->deckSize(), '==', 51, 'deck size decrease by one');
   $deck->getCard() for(1 .. 11);
   cmp_ok($deck->deckSize(), '==', 40, ' 11 more gone ' );
};

subtest 'discard dies if its argument is not a card' => sub{
   my $deck = Poker::Deck->new();

   throws_ok { $deck->discard() } qr/invalid/, "dies for invalid card";
};

subtest 'discard dies if you discard a card that does not belong to it' => sub{
   my $deck = Poker::Deck->new();
   throws_ok { $deck->discard(Poker::Card->new()); } qr/card.*?deck/, "Cannot discard a card not belonging to this deck";
};

subtest 'burnCard dcreases deck size'=>sub{
   my $deck = Poker::Deck->new();
   cmp_ok($deck->deckSize(), '==', 52, 'deck starts with 52 cards');
   $deck->burnCard();
   cmp_ok($deck->deckSize(), '==', 51, 'deck size decrease by one');
   $deck->burnCard() for(1 .. 11);
   cmp_ok($deck->deckSize(), '==', 40, ' 11 more gone ' );
};

subtest 'burnSize increases when a card is discarded'=>sub{
   my $deck = Poker::Deck->new();
   cmp_ok($deck->burnSize(), '==', 0, 'burn starts at 0');
   my $card = $deck->getCard();
   $deck->discard($card);
   cmp_ok($deck->burnSize(), '==', 1, 'burnSize increased');
};

subtest 'burnCard moves a card directly from deck to burn pile'=>sub{
   my $deck = Poker::Deck->new();
   cmp_ok($deck->burnSize(), '==', 0, 'burn starts at 0');
   my $card = $deck->burnCard();
   cmp_ok($deck->burnSize(), '==', 1, 'burnSize increased');
};

subtest 'getCard returns undef if no cards remain' => sub{
   my $deck = Poker::Deck->new();
   while($deck->deckSize() >0){
      $deck->burnCard();
   }

   ok(!$deck->getCard());
};


done_testing();
